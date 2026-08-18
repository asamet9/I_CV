using ICV.Application.DTOs.AI;
using ICV.Application.DTOs.CourseRecommendation;
using ICV.Application.Interfaces.AI;
using ICV.Application.Interfaces.Services;
using ICV.Application.Interfaces.UnitOfWork;
using ICV.Domain.Entities;
using ICV.Domain.Enums;

namespace ICV.Application.Services
{
    public class CourseRecommendationService : ICourseRecommendationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAiProvider _aiProvider;

        public CourseRecommendationService(
            IUnitOfWork unitOfWork,
            IAiProvider aiProvider)
        {
            _unitOfWork = unitOfWork;
            _aiProvider = aiProvider;
        }


        // =========================================================
        // AI → GELİŞİM HEDEFİNE KURS ÖNERİLERİ ÜRET
        // =========================================================

        public async Task<IEnumerable<CourseRecommendationResponseDto>>
            GenerateForGoalAsync(
                int skillDevelopmentGoalId,
                int userId)
        {
            var goal = await _unitOfWork.SkillDevelopmentGoals
                .FirstOrDefaultAsync(x =>
                    x.Id == skillDevelopmentGoalId &&
                    x.UserId == userId);

            if (goal == null)
            {
                throw new UnauthorizedAccessException(
                    "Bu gelişim hedefine erişim yetkiniz yok.");
            }

            var request = new AiCourseSearchRequestDto
            {
                SkillName = goal.SkillName,
                CurrentLevel = (int)goal.CurrentLevel,
                TargetLevel = (int)goal.TargetLevel,
                PreferredDuration = (int)goal.PreferredDuration,
                WantsPaidCourse = goal.WantsPaidCourse,
                WantsCertificate = goal.WantsCertificate,
                Purpose = goal.Purpose
            };

            var aiRecommendations =
                await _aiProvider.GenerateCourseRecommendationsAsync(
                    request);

            var results =
                new List<CourseRecommendationResponseDto>();


            foreach (var aiRecommendation in aiRecommendations)
            {
                if (string.IsNullOrWhiteSpace(aiRecommendation.Url))
                    continue;
                if (!Uri.TryCreate(
        aiRecommendation.Url,
        UriKind.Absolute,
        out var courseUri) ||
    courseUri.Scheme != Uri.UriSchemeHttps)
                {
                    continue;
                }

                // =================================================
                // COURSE KONTROLÜ
                // =================================================

                var existingCourse =
                    await _unitOfWork.Courses
                        .FirstOrDefaultAsync(x =>
                            x.Url == aiRecommendation.Url);

                Course course;

                if (existingCourse == null)
                {
                    course = new Course
                    {
                        Title = aiRecommendation.Title,
                        Provider = aiRecommendation.Provider,
                        Url = aiRecommendation.Url,

                        Category = aiRecommendation.Category,

                        Level = (SkillLevel)aiRecommendation.Level,

                        IsFree = aiRecommendation.IsFree,

                        DurationHours = aiRecommendation.DurationHours,

                        IsActive = true
                    };

                    await _unitOfWork.Courses
                        .AddAsync(course);

                    await _unitOfWork
                        .SaveChangesAsync();
                }
                else
                {
                    course = existingCourse;
                }


                // =================================================
                // DUPLICATE KONTROLÜ
                // =================================================

                var alreadyRecommended =
                    await _unitOfWork.CourseRecommendations
                        .AnyAsync(x =>
                            x.SkillDevelopmentGoalId ==
                                skillDevelopmentGoalId &&
                            x.CourseId == course.Id);

                if (alreadyRecommended)
                    continue;


                // =================================================
                // COURSE RECOMMENDATION
                // =================================================

                var recommendation =
                    new CourseRecommendation
                    {
                        SkillDevelopmentGoalId =
                            skillDevelopmentGoalId,

                        CourseId =
                            course.Id
                    };

                await _unitOfWork.CourseRecommendations
                    .AddAsync(recommendation);

                await _unitOfWork
                    .SaveChangesAsync();


                results.Add(
                    MapToResponse(
                        recommendation,
                        course));
            }


            return results;
        }


        // =========================================================
        // MANUEL COURSE RECOMMENDATION OLUŞTUR
        // =========================================================

        public async Task<CourseRecommendationResponseDto>
            CreateAsync(
                CreateCourseRecommendationRequestDto request,
                int userId)
        {
            var goal =
                await _unitOfWork.SkillDevelopmentGoals
                    .FirstOrDefaultAsync(x =>
                        x.Id == request.SkillDevelopmentGoalId &&
                        x.UserId == userId);

            if (goal == null)
            {
                throw new UnauthorizedAccessException(
                    "Bu gelişim hedefine erişim yetkiniz yok.");
            }


            var course =
                await _unitOfWork.Courses
                    .FirstOrDefaultAsync(x =>
                        x.Id == request.CourseId &&
                        x.IsActive);

            if (course == null)
            {
                throw new KeyNotFoundException(
                    "Önerilmek istenen kurs bulunamadı veya aktif değil.");
            }


            var alreadyRecommended =
                await _unitOfWork.CourseRecommendations
                    .AnyAsync(x =>
                        x.SkillDevelopmentGoalId ==
                            request.SkillDevelopmentGoalId &&
                        x.CourseId ==
                            request.CourseId);

            if (alreadyRecommended)
            {
                throw new InvalidOperationException(
                    "Bu kurs zaten bu gelişim hedefi için önerilmiş.");
            }


            var courseRecommendation =
                new CourseRecommendation
                {
                    SkillDevelopmentGoalId =
                        request.SkillDevelopmentGoalId,

                    CourseId =
                        request.CourseId
                };


            await _unitOfWork.CourseRecommendations
                .AddAsync(courseRecommendation);

            await _unitOfWork
                .SaveChangesAsync();


            return MapToResponse(
                courseRecommendation,
                course);
        }


        // =========================================================
        // GELİŞİM HEDEFİNİN KURSLARINI GETİR
        // =========================================================

        public async Task<IEnumerable<CourseRecommendationResponseDto>>
            GetAllAsync(
                int skillDevelopmentGoalId,
                int userId)
        {
            var goal =
                await _unitOfWork.SkillDevelopmentGoals
                    .FirstOrDefaultAsync(x =>
                        x.Id == skillDevelopmentGoalId &&
                        x.UserId == userId);

            if (goal == null)
            {
                throw new UnauthorizedAccessException(
                    "Bu gelişim hedefine erişim yetkiniz yok.");
            }


            var recommendations =
                await _unitOfWork.CourseRecommendations
                    .FindAsync(x =>
                        x.SkillDevelopmentGoalId ==
                            skillDevelopmentGoalId);


            var result =
                new List<CourseRecommendationResponseDto>();


            foreach (var recommendation in recommendations)
            {
                var course =
                    await _unitOfWork.Courses
                        .FirstOrDefaultAsync(x =>
                            x.Id == recommendation.CourseId);

                if (course == null)
                    continue;


                result.Add(
                    MapToResponse(
                        recommendation,
                        course));
            }


            return result;
        }


        // =========================================================
        // TEK COURSE RECOMMENDATION GETİR
        // =========================================================

        public async Task<CourseRecommendationResponseDto?>
            GetByIdAsync(
                int courseRecommendationId,
                int userId)
        {
            var recommendation =
                await _unitOfWork.CourseRecommendations
                    .FirstOrDefaultAsync(x =>
                        x.Id == courseRecommendationId &&
                        x.SkillDevelopmentGoal.UserId == userId);

            if (recommendation == null)
                return null;


            var course =
                await _unitOfWork.Courses
                    .FirstOrDefaultAsync(x =>
                        x.Id == recommendation.CourseId);

            if (course == null)
                return null;


            return MapToResponse(
                recommendation,
                course);
        }


        // =========================================================
        // UPDATE
        // =========================================================

        public async Task<CourseRecommendationResponseDto?>
            UpdateAsync(
                int courseRecommendationId,
                UpdateCourseRecommendationRequestDto request,
                int userId)
        {
            var recommendation =
                await _unitOfWork.CourseRecommendations
                    .FirstOrDefaultAsync(x =>
                        x.Id == courseRecommendationId &&
                        x.SkillDevelopmentGoal.UserId == userId);

            if (recommendation == null)
                return null;


            var course =
                await _unitOfWork.Courses
                    .FirstOrDefaultAsync(x =>
                        x.Id == request.CourseId &&
                        x.IsActive);

            if (course == null)
            {
                throw new KeyNotFoundException(
                    "Seçilen kurs bulunamadı veya aktif değil.");
            }


            var alreadyRecommended =
                await _unitOfWork.CourseRecommendations
                    .AnyAsync(x =>
                        x.Id != courseRecommendationId &&
                        x.SkillDevelopmentGoalId ==
                            recommendation.SkillDevelopmentGoalId &&
                        x.CourseId ==
                            request.CourseId);

            if (alreadyRecommended)
            {
                throw new InvalidOperationException(
                    "Bu kurs zaten bu gelişim hedefi için önerilmiş.");
            }


            recommendation.CourseId =
                request.CourseId;


            _unitOfWork.CourseRecommendations
                .Update(recommendation);

            await _unitOfWork
                .SaveChangesAsync();


            return MapToResponse(
                recommendation,
                course);
        }


        // =========================================================
        // DELETE
        // =========================================================

        public async Task<bool>
            DeleteAsync(
                int courseRecommendationId,
                int userId)
        {
            var recommendation =
                await _unitOfWork.CourseRecommendations
                    .FirstOrDefaultAsync(x =>
                        x.Id == courseRecommendationId &&
                        x.SkillDevelopmentGoal.UserId == userId);

            if (recommendation == null)
                return false;


            _unitOfWork.CourseRecommendations
                .Delete(recommendation);

            await _unitOfWork
                .SaveChangesAsync();


            return true;
        }


        // =========================================================
        // RESPONSE MAPPING
        // =========================================================

        private static CourseRecommendationResponseDto
            MapToResponse(
                CourseRecommendation recommendation,
                Course course)
        {
            return new CourseRecommendationResponseDto
            {
                Id = recommendation.Id,

                SkillDevelopmentGoalId =
           recommendation.SkillDevelopmentGoalId,

                CourseId =
           recommendation.CourseId,

                Title =
           course.Title,

                Provider =
           course.Provider,

                Url =
           course.Url,

                Category =
           course.Category,

                Price =
           default,

                Level =
           default,

                DurationWeeks =
           0,

                CreatedAt =
           recommendation.CreatedAt
            };
        }
    }
}