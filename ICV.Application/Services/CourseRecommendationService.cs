using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Application.DTOs.CourseRecommendation;
using ICV.Application.Interfaces.Services;
using ICV.Application.Interfaces.UnitOfWork;
using ICV.Domain.Entities;

namespace ICV.Application.Services
{
    public class CourseRecommendationService : ICourseRecommendationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseRecommendationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CourseRecommendationResponseDto> CreateAsync(
            CreateCourseRecommendationRequestDto request,
            int userId)
        {
            // Önce SkillSuggestion'ın gerçekten giriş yapan kullanıcıya
            // ait bir CV üzerinden geldiğini kontrol ediyoruz.
            var skillSuggestion = await _unitOfWork.SkillSuggestions
                .FirstOrDefaultAsync(x =>
                    x.Id == request.SkillSuggestionId &&
                    x.Cv.UserId == userId);

            if (skillSuggestion == null)
                throw new UnauthorizedAccessException(
                    "Bu skill önerisine erişim yetkiniz yok.");

            var course = new CourseRecommendation
            {
                SkillSuggestionId = request.SkillSuggestionId,
                Title = request.Title,
                Provider = request.Provider,
                Price = request.Price,
                Level = request.Level,
                DurationWeeks = request.DurationWeeks,
                Url = request.Url
            };

            await _unitOfWork.CourseRecommendations
                .AddAsync(course);

            await _unitOfWork.SaveChangesAsync();

            return new CourseRecommendationResponseDto
            {
                Id = course.Id,
                SkillSuggestionId = course.SkillSuggestionId,
                Title = course.Title,
                Provider = course.Provider,
                Price = course.Price,
                Level = course.Level,
                DurationWeeks = course.DurationWeeks,
                Url = course.Url,
                CreatedAt = course.CreatedAt
            };
        }

        public async Task<IEnumerable<CourseRecommendationResponseDto>> GetAllAsync(
            int skillSuggestionId,
            int userId)
        {
            // SkillSuggestion'ın kullanıcıya ait olduğunu kontrol ediyoruz.
            var skillSuggestion = await _unitOfWork.SkillSuggestions
                .FirstOrDefaultAsync(x =>
                    x.Id == skillSuggestionId &&
                    x.Cv.UserId == userId);

            if (skillSuggestion == null)
                throw new UnauthorizedAccessException(
                    "Bu skill önerisine erişim yetkiniz yok.");

            var courses = await _unitOfWork.CourseRecommendations
                .FindAsync(x =>
                    x.SkillSuggestionId == skillSuggestionId);

            return courses.Select(x => new CourseRecommendationResponseDto
            {
                Id = x.Id,
                SkillSuggestionId = x.SkillSuggestionId,
                Title = x.Title,
                Provider = x.Provider,
                Price = x.Price,
                Level = x.Level,
                DurationWeeks = x.DurationWeeks,
                Url = x.Url,
                CreatedAt = x.CreatedAt
            });
        }

        public async Task<CourseRecommendationResponseDto?> GetByIdAsync(
            int courseRecommendationId,
            int userId)
        {
            var course = await _unitOfWork.CourseRecommendations
                .FirstOrDefaultAsync(x =>
                    x.Id == courseRecommendationId &&
                    x.SkillSuggestion.Cv.UserId == userId);

            if (course == null)
                return null;

            return new CourseRecommendationResponseDto
            {
                Id = course.Id,
                SkillSuggestionId = course.SkillSuggestionId,
                Title = course.Title,
                Provider = course.Provider,
                Price = course.Price,
                Level = course.Level,
                DurationWeeks = course.DurationWeeks,
                Url = course.Url,
                CreatedAt = course.CreatedAt
            };
        }

        public async Task<CourseRecommendationResponseDto?> UpdateAsync(
            int courseRecommendationId,
            UpdateCourseRecommendationRequestDto request,
            int userId)
        {
            var course = await _unitOfWork.CourseRecommendations
                .FirstOrDefaultAsync(x =>
                    x.Id == courseRecommendationId &&
                    x.SkillSuggestion.Cv.UserId == userId);

            if (course == null)
                return null;

            course.Title = request.Title;
            course.Provider = request.Provider;
            course.Price = request.Price;
            course.Level = request.Level;
            course.DurationWeeks = request.DurationWeeks;
            course.Url = request.Url;

            _unitOfWork.CourseRecommendations.Update(course);

            await _unitOfWork.SaveChangesAsync();

            return new CourseRecommendationResponseDto
            {
                Id = course.Id,
                SkillSuggestionId = course.SkillSuggestionId,
                Title = course.Title,
                Provider = course.Provider,
                Price = course.Price,
                Level = course.Level,
                DurationWeeks = course.DurationWeeks,
                Url = course.Url,
                CreatedAt = course.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(
            int courseRecommendationId,
            int userId)
        {
            var course = await _unitOfWork.CourseRecommendations
                .FirstOrDefaultAsync(x =>
                    x.Id == courseRecommendationId &&
                    x.SkillSuggestion.Cv.UserId == userId);

            if (course == null)
                return false;

            _unitOfWork.CourseRecommendations.Delete(course);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}

