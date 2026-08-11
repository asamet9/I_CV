using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Application.DTOs.CourseRecommendation;

namespace ICV.Application.Interfaces.Services
{
    public interface ICourseRecommendationService
    {
        Task<CourseRecommendationResponseDto> CreateAsync(
            CreateCourseRecommendationRequestDto request,
            int userId);

        Task<IEnumerable<CourseRecommendationResponseDto>> GetAllAsync(
            int skillSuggestionId,
            int userId);

        Task<CourseRecommendationResponseDto?> GetByIdAsync(
            int courseRecommendationId,
            int userId);

        Task<CourseRecommendationResponseDto?> UpdateAsync(
            int courseRecommendationId,
            UpdateCourseRecommendationRequestDto request,
            int userId);

        Task<bool> DeleteAsync(
            int courseRecommendationId,
            int userId);
    }
}
