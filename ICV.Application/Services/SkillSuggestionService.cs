using ICV.Application.DTOs.CvAnalysis;
using ICV.Application.DTOs.SkillSuggestion;
using ICV.Application.Interfaces.AI;
using ICV.Application.Interfaces.Services;
using ICV.Application.Interfaces.UnitOfWork;
using ICV.Domain.Entities;
using ICV.Domain.Enums;

namespace ICV.Application.Services
{
    public class SkillSuggestionService : ISkillSuggestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAiProvider _aiProvider;

        public SkillSuggestionService(
            IUnitOfWork unitOfWork,
            IAiProvider aiProvider)
        {
            _unitOfWork = unitOfWork;
            _aiProvider = aiProvider;
        }

        public async Task<SkillSuggestionResponseDto> CreateAsync(
            CreateSkillSuggestionRequestDto request,
            int userId)
        {
            var cv = await _unitOfWork.Cvs
                .FirstOrDefaultAsync(x =>
                    x.Id == request.CvId &&
                    x.UserId == userId);

            if (cv == null)
            {
                throw new UnauthorizedAccessException(
                    "Bu CV'ye erişim yetkiniz yok.");
            }

            var suggestion = new SkillSuggestion
            {
                CvId = request.CvId,
                SuggestedSkill = request.SuggestedSkill,
                Reason = request.Reason,
                Category = request.Category,
                Status = (SuggestionStatus)request.Status
            };

            await _unitOfWork.SkillSuggestions.AddAsync(suggestion);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(suggestion);
        }

        public async Task<IEnumerable<SkillSuggestionResponseDto>> GetAllAsync(
            int cvId,
            int userId)
        {
            var cv = await _unitOfWork.Cvs
                .FirstOrDefaultAsync(x =>
                    x.Id == cvId &&
                    x.UserId == userId);

            if (cv == null)
            {
                throw new UnauthorizedAccessException(
                    "Bu CV'ye erişim yetkiniz yok.");
            }

            var suggestions = await _unitOfWork.SkillSuggestions
                .FindAsync(x => x.CvId == cvId);

            return suggestions.Select(MapToDto);
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

            return MapToDto(suggestion);
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

            return MapToDto(suggestion);
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

        public async Task<IEnumerable<SkillSuggestionResponseDto>> GenerateFromAnalysisAsync(
            int cvId,
            IEnumerable<MissingSkillDto> missingSkills,
            int userId)
        {
            var cv = await _unitOfWork.Cvs
                .FirstOrDefaultAsync(x =>
                    x.Id == cvId &&
                    x.UserId == userId);

            if (cv == null)
            {
                throw new UnauthorizedAccessException(
                    "Bu CV'ye erişim yetkiniz yok.");
            }

            var suggestions = new List<SkillSuggestion>();

            foreach (var skill in missingSkills)
            {
                if (string.IsNullOrWhiteSpace(skill.Skill))
                    continue;

                var exists = await _unitOfWork.SkillSuggestions
                    .AnyAsync(x =>
                        x.CvId == cvId &&
                        x.SuggestedSkill == skill.Skill);

                if (exists)
                    continue;

                var suggestion = new SkillSuggestion
                {
                    CvId = cvId,
                    SuggestedSkill = skill.Skill,
                    Reason =
                        "Bu yetenek seçilen meslek için gelişim açısından faydalıdır.",
                    Category = skill.Category,
                    Status = SuggestionStatus.Pending
                };

                suggestions.Add(suggestion);
            }

            foreach (var suggestion in suggestions)
            {
                await _unitOfWork.SkillSuggestions
                    .AddAsync(suggestion);
            }

            if (suggestions.Count > 0)
            {
                await _unitOfWork.SaveChangesAsync();
            }

            return suggestions.Select(MapToDto);
        }

        public async Task<IEnumerable<SkillSuggestionResponseDto>> GenerateFromAiAsync(
            int cvId,
            string cvContent,
            string professionName,
            int userId)
        {
            var cv = await _unitOfWork.Cvs
                .FirstOrDefaultAsync(x =>
                    x.Id == cvId &&
                    x.UserId == userId);

            if (cv == null)
            {
                throw new UnauthorizedAccessException(
                    "Bu CV'ye erişim yetkiniz yok.");
            }

            if (string.IsNullOrWhiteSpace(cvContent))
            {
                throw new ArgumentException(
                    "CV içeriği boş olamaz.",
                    nameof(cvContent));
            }

            if (string.IsNullOrWhiteSpace(professionName))
            {
                throw new ArgumentException(
                    "Meslek bilgisi boş olamaz.",
                    nameof(professionName));
            }

            // ---------------------------------------------------------
            // AI'YA CV'Yİ GÖNDER
            // ---------------------------------------------------------

            var aiSuggestions =
                await _aiProvider.GenerateSkillSuggestionsAsync(
                    cvContent,
                    professionName);

            var suggestions = new List<SkillSuggestion>();

            // ---------------------------------------------------------
            // AI ÖNERİLERİNİ DB'YE KAYDET
            // ---------------------------------------------------------

            foreach (var aiSuggestion in aiSuggestions)
            {
                if (string.IsNullOrWhiteSpace(aiSuggestion.Skill))
                    continue;

                var skillName = aiSuggestion.Skill.Trim();

                var exists = await _unitOfWork.SkillSuggestions
                    .AnyAsync(x =>
                        x.CvId == cvId &&
                        x.SuggestedSkill.ToLower() ==
                        skillName.ToLower());

                if (exists)
                    continue;

                var suggestion = new SkillSuggestion
                {
                    CvId = cvId,

                    SuggestedSkill = skillName,

                    Reason = string.IsNullOrWhiteSpace(aiSuggestion.Reason)
                        ? "Bu skill adayın kariyer gelişimi için önerilmiştir."
                        : aiSuggestion.Reason,

                    Category = aiSuggestion.Category,

                    RecommendedTargetLevel =
                        aiSuggestion.RecommendedTargetLevel,

                    Status = SuggestionStatus.Pending
                };

                suggestions.Add(suggestion);
            }

            foreach (var suggestion in suggestions)
            {
                await _unitOfWork.SkillSuggestions
                    .AddAsync(suggestion);
            }

            if (suggestions.Count > 0)
            {
                await _unitOfWork.SaveChangesAsync();
            }

            return suggestions.Select(MapToDto);
        }

        private static SkillSuggestionResponseDto MapToDto(
            SkillSuggestion suggestion)
        {
            return new SkillSuggestionResponseDto
            {
                Id = suggestion.Id,
                CvId = suggestion.CvId,
                SuggestedSkill = suggestion.SuggestedSkill,
                Reason = suggestion.Reason,
                Category = suggestion.Category,
                RecommendedTargetLevel =
                    suggestion.RecommendedTargetLevel,
                Status = (int)suggestion.Status,
                CreatedAt = suggestion.CreatedAt
            };
        }
    }
}