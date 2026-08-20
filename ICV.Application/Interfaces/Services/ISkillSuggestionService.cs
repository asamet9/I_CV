using ICV.Application.DTOs.CvAnalysis;
using ICV.Application.DTOs.SkillSuggestion;

namespace ICV.Application.Interfaces.Services
{
    public interface ISkillSuggestionService
    {
        Task<SkillSuggestionResponseDto> CreateAsync(
            CreateSkillSuggestionRequestDto request,
            int userId);

        Task<IEnumerable<SkillSuggestionResponseDto>> GetAllAsync(
            int cvId,
            int userId);

        Task<SkillSuggestionResponseDto?> GetByIdAsync(
            int suggestionId,
            int userId);

        Task<SkillSuggestionResponseDto?> UpdateAsync(
            int suggestionId,
            UpdateSkillSuggestionRequestDto request,
            int userId);

        Task<bool> DeleteAsync(
            int suggestionId,
            int userId);

        Task<IEnumerable<SkillSuggestionResponseDto>> GenerateFromAnalysisAsync(
            int cvId,
            IEnumerable<MissingSkillDto> missingSkills,
            int userId);

        Task<IEnumerable<SkillSuggestionResponseDto>> GenerateFromAiAsync(
            int cvId,
            string cvContent,
            string professionName,
            int userId);
    }
}