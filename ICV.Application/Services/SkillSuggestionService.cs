using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
