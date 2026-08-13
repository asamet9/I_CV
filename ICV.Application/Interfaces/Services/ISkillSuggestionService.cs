using ICV.Application.DTOs.CvAnalysis;
using ICV.Application.DTOs.SkillSuggestion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // CV analizi sonucunda eksik kalan yetenekler için
        // otomatik SkillSuggestion kayıtları oluşturur.
        Task<IEnumerable<SkillSuggestionResponseDto>> GenerateFromAnalysisAsync(
            int cvId,
            IEnumerable<MissingSkillDto> missingSkills,
            int userId);
    }
}
