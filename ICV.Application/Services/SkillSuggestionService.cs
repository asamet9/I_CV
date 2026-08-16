using ICV.Application.DTOs.CvAnalysis;
using ICV.Application.DTOs.SkillSuggestion;
using ICV.Application.Interfaces.AI; // IAiProvider interface'ine erişmemizi sağlar.
using ICV.Application.Interfaces.Services;
using ICV.Application.Interfaces.UnitOfWork;
using ICV.Domain.Entities;
using ICV.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Application.Services
{
    public class SkillSuggestionService : ISkillSuggestionService
    {

        private readonly IUnitOfWork _unitOfWork; // Veritabanı işlemlerini yöneten UnitOfWork nesnesidir.
        private readonly IAiProvider _aiProvider; // AI işlemlerini gerçekleştiren provider'ı tutar.

        public SkillSuggestionService(
            IUnitOfWork unitOfWork,
            IAiProvider aiProvider)
        {
            _unitOfWork = unitOfWork; // Dependency Injection tarafından verilen UnitOfWork'ü saklar.
            _aiProvider = aiProvider; // Dependency Injection tarafından verilen AI provider'ı saklar.
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

        // Gemini AI tarafından oluşturulan skill önerilerini
        // SkillSuggestion kayıtlarına dönüştürür.
        public async Task<IEnumerable<SkillSuggestionResponseDto>> GenerateFromAiAsync(
            int cvId,
            string cvContent,
            string professionName,
            int userId)
        {
            // CV'nin giriş yapan kullanıcıya ait olup olmadığını kontrol ediyoruz.
            var cv = await _unitOfWork.Cvs
                .FirstOrDefaultAsync(x =>
                    x.Id == cvId &&
                    x.UserId == userId);

            // CV bulunamadıysa veya kullanıcı bu CV'ye ait değilse işlemi durduruyoruz.
            if (cv == null)
                throw new UnauthorizedAccessException(
                    "Bu CV'ye erişim yetkiniz yok.");

            // CV içeriğinin boş olup olmadığını kontrol ediyoruz.
            if (string.IsNullOrWhiteSpace(cvContent))
                throw new ArgumentException(
                    "CV içeriği boş olamaz.",
                    nameof(cvContent));

            // Meslek bilgisinin boş olup olmadığını kontrol ediyoruz.
            if (string.IsNullOrWhiteSpace(professionName))
                throw new ArgumentException(
                    "Meslek bilgisi boş olamaz.",
                    nameof(professionName));

            // AI provider'a CV'yi ve mesleği gönderiyoruz.
            // Burada GeminiAiProvider kullanıldığını service bilmiyor.
            var aiSuggestions = await _aiProvider
                .GenerateSkillSuggestionsAsync(
                    cvContent,
                    professionName);

            // AI'dan gelen önerileri SkillSuggestion entity'lerine
            // dönüştürmeden önce burada tutacağız.
            var suggestions = new List<SkillSuggestion>();

            // Gemini'nin döndürdüğü her skill önerisini işliyoruz.
            foreach (var aiSuggestion in aiSuggestions)
            {
                // Skill boşsa bu öneriyi veritabanına kaydetmiyoruz.
                if (string.IsNullOrWhiteSpace(aiSuggestion.Skill))
                    continue;

                // Aynı skill daha önce bu CV için önerilmişse
                // duplicate kayıt oluşturmuyoruz.
                var suggestionExists = await _unitOfWork.SkillSuggestions
                    .AnyAsync(x =>
                        x.CvId == cvId &&
                        x.SuggestedSkill == aiSuggestion.Skill);

                // Skill zaten varsa bir sonraki öneriye geçiyoruz.
                if (suggestionExists)
                    continue;

                // AI önerisini Domain entity'sine dönüştürüyoruz.
                var suggestion = new SkillSuggestion
                {
                    CvId = cvId, // Önerinin hangi CV'ye ait olduğunu belirtir.

                    SuggestedSkill = aiSuggestion.Skill, // Gemini'nin önerdiği skill'i kaydeder.

                    Reason = aiSuggestion.Reason, // Gemini'nin skill'i neden önerdiğini kaydeder.

                    Category = aiSuggestion.Category, // Gemini'nin belirlediği kategoriyi kaydeder.

                    Status = SuggestionStatus.Pending // Yeni AI önerilerinin başlangıç durumudur.
                };

                // Oluşturduğumuz entity'yi geçici listeye ekliyoruz.
                suggestions.Add(suggestion);
            }

            // Oluşturulan tüm skill önerilerini veritabanına ekliyoruz.
            foreach (var suggestion in suggestions)
            {
                await _unitOfWork.SkillSuggestions
                    .AddAsync(suggestion);
            }

            // Tüm yeni SkillSuggestion kayıtlarını tek seferde kaydediyoruz.
            await _unitOfWork.SaveChangesAsync();

            // Domain entity'lerini API'nin kullanacağı response DTO'larına dönüştürüyoruz.
            return suggestions.Select(x => new SkillSuggestionResponseDto
            {
                Id = x.Id, // Veritabanında oluşturulan ID'yi döndürür.

                CvId = x.CvId, // Önerinin ait olduğu CV ID'sini döndürür.

                SuggestedSkill = x.SuggestedSkill, // Önerilen skill'i döndürür.

                Reason = x.Reason, // Skill'in önerilme nedenini döndürür.

                Category = x.Category, // Skill kategorisini döndürür.

                Status = (int)x.Status, // Enum değerini API response'una integer olarak aktarır.

                CreatedAt = x.CreatedAt // Kaydın oluşturulma tarihini döndürür.
            });
        }

    }
}