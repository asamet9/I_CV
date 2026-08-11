using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
