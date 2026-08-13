using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Application.DTOs.CvAnalysis;
using ICV.Application.DTOs.SkillSuggestion;
using ICV.Application.Interfaces.Services;
using ICV.Application.Interfaces.UnitOfWork;
using ICV.Domain.Entities;
using ICV.Domain.Enums;

namespace ICV.Application.Services
{
    public class SkillSuggestionService : ISkillSuggestionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SkillSuggestionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SkillSuggestionResponseDto> CreateAsync(
            CreateSkillSuggestionRequestDto request,
            int userId)
        {
            // CV'nin giriş yapan kullanıcıya ait olup olmadığını kontrol ediyoruz.
            var cv = await _unitOfWork.Cvs
                .FirstOrDefaultAsync(x =>
                    x.Id == request.CvId &&
                    x.UserId == userId);

            if (cv == null)
                throw new UnauthorizedAccessException(
                    "Bu CV'ye erişim yetkiniz yok.");

            var suggestion = new SkillSuggestion
            {
                CvId = request.CvId,
                SuggestedSkill = request.SuggestedSkill,
                Reason = request.Reason,
                Category = request.Category,
                Status = (SuggestionStatus)request.Status
            };

            await _unitOfWork.SkillSuggestions
                .AddAsync(suggestion);

            await _unitOfWork.SaveChangesAsync();

            return new SkillSuggestionResponseDto
            {
                Id = suggestion.Id,
                CvId = suggestion.CvId,
                SuggestedSkill = suggestion.SuggestedSkill,
                Reason = suggestion.Reason,
                Category = suggestion.Category,
                Status = (int)suggestion.Status,
                CreatedAt = suggestion.CreatedAt
            };
        }

        public async Task<IEnumerable<SkillSuggestionResponseDto>> GetAllAsync(
            int cvId,
            int userId)
        {
            // Sadece kullanıcının kendi CV'sine ait önerileri getiriyoruz.
            var cv = await _unitOfWork.Cvs
                .FirstOrDefaultAsync(x =>
                    x.Id == cvId &&
                    x.UserId == userId);

            if (cv == null)
                throw new UnauthorizedAccessException(
                    "Bu CV'ye erişim yetkiniz yok.");

            var suggestions = await _unitOfWork.SkillSuggestions
                .FindAsync(x => x.CvId == cvId);

            return suggestions.Select(x => new SkillSuggestionResponseDto
            {
                Id = x.Id,
                CvId = x.CvId,
                SuggestedSkill = x.SuggestedSkill,
                Reason = x.Reason,
                Category = x.Category,
                Status = (int)x.Status,
                CreatedAt = x.CreatedAt
            });
        }

        public async Task<SkillSuggestionResponseDto?> GetByIdAsync(
            int suggestionId,
            int userId)
        {
            var suggestion = await _unitOfWork.SkillSuggestions
                .FirstOrDefaultAsync(x =>
                    x.Id == suggestionId &&
                    x.Cv.UserId == userId);

            if (suggestion == null)
                return null;

            return new SkillSuggestionResponseDto
            {
                Id = suggestion.Id,
                CvId = suggestion.CvId,
                SuggestedSkill = suggestion.SuggestedSkill,
                Reason = suggestion.Reason,
                Category = suggestion.Category,
                Status = (int)suggestion.Status,
                CreatedAt = suggestion.CreatedAt
            };
        }

        public async Task<SkillSuggestionResponseDto?> UpdateAsync(
            int suggestionId,
            UpdateSkillSuggestionRequestDto request,
            int userId)
        {
            var suggestion = await _unitOfWork.SkillSuggestions
                .FirstOrDefaultAsync(x =>
                    x.Id == suggestionId &&
                    x.Cv.UserId == userId);

            if (suggestion == null)
                return null;

            suggestion.SuggestedSkill = request.SuggestedSkill;
            suggestion.Reason = request.Reason;
            suggestion.Category = request.Category;
            suggestion.Status = (SuggestionStatus)request.Status;

            _unitOfWork.SkillSuggestions.Update(suggestion);

            await _unitOfWork.SaveChangesAsync();

            return new SkillSuggestionResponseDto
            {
                Id = suggestion.Id,
                CvId = suggestion.CvId,
                SuggestedSkill = suggestion.SuggestedSkill,
                Reason = suggestion.Reason,
                Category = suggestion.Category,
                Status = (int)suggestion.Status,
                CreatedAt = suggestion.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(
            int suggestionId,
            int userId)
        {
            var suggestion = await _unitOfWork.SkillSuggestions
                .FirstOrDefaultAsync(x =>
                    x.Id == suggestionId &&
                    x.Cv.UserId == userId);

            if (suggestion == null)
                return false;

            _unitOfWork.SkillSuggestions.Delete(suggestion);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        // CV analizinde eksik bulunan yetenekleri otomatik olarak
        // SkillSuggestion kayıtlarına dönüştürür.
        public async Task<IEnumerable<SkillSuggestionResponseDto>> GenerateFromAnalysisAsync(
            int cvId,
            IEnumerable<MissingSkillDto> missingSkills,
            int userId)
        {
            // CV'nin giriş yapan kullanıcıya ait olup olmadığını kontrol ediyoruz.
            var cv = await _unitOfWork.Cvs
                .FirstOrDefaultAsync(x =>
                    x.Id == cvId &&
                    x.UserId == userId);

            if (cv == null)
                throw new UnauthorizedAccessException(
                    "Bu CV'ye erişim yetkiniz yok.");

            // Oluşturacağımız önerileri burada tutacağız.
            var suggestions = new List<SkillSuggestion>();

            // Analiz sonucunda bulunan her eksik skill için
            // ayrı bir SkillSuggestion oluşturuyoruz.
            foreach (var skill in missingSkills)
            {
                // Skill boşsa kaydetmiyoruz.
                if (string.IsNullOrWhiteSpace(skill.Skill))
                    continue;

                // Aynı skill daha önce bu CV için önerilmişse
                // tekrar kayıt oluşturmuyoruz.
                var suggestionExists = await _unitOfWork.SkillSuggestions
                    .AnyAsync(x =>
                        x.CvId == cvId &&
                        x.SuggestedSkill == skill.Skill);

                if (suggestionExists)
                    continue;

                // Eksik skill için yeni öneri oluşturuyoruz.
                var suggestion = new SkillSuggestion
                {
                    CvId = cvId,

                    // MissingSkillDto içerisindeki gerçek skill.
                    SuggestedSkill = skill.Skill,

                    Reason =
                        "Bu yetenek seçilen meslek için gerekli ancak CV'de bulunamadı.",

                    // Artık sabit "Technical" kullanmıyoruz.
                    // QuestionTemplate'dan gelen kategori kullanılıyor.
                    Category = skill.Category,

                    Status = SuggestionStatus.Pending
                };

                suggestions.Add(suggestion);
            }

            // Yeni oluşturulan önerilerin tamamını veritabanına ekliyoruz.
            foreach (var suggestion in suggestions)
            {
                await _unitOfWork.SkillSuggestions
                    .AddAsync(suggestion);
            }

            // Tüm yeni kayıtları kaydediyoruz.
            await _unitOfWork.SaveChangesAsync();

            // Entity'leri DTO'lara çeviriyoruz.
            return suggestions.Select(x => new SkillSuggestionResponseDto
            {
                Id = x.Id,
                CvId = x.CvId,
                SuggestedSkill = x.SuggestedSkill,
                Reason = x.Reason,
                Category = x.Category,
                Status = (int)x.Status,
                CreatedAt = x.CreatedAt
            });
        }
    }
}