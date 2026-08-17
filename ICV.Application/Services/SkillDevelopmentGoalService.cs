using ICV.Application.DTOs.SkillDevelopmentGoal;
using ICV.Application.Interfaces.Services;
using ICV.Application.Interfaces.UnitOfWork;
using ICV.Domain.Entities;
using ICV.Domain.Enums;

namespace ICV.Application.Services
{
    /// <summary>
    /// Kullanıcının skill geliştirme hedeflerini yöneten servistir.
    /// </summary>
    public class SkillDevelopmentGoalService
        : ISkillDevelopmentGoalService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SkillDevelopmentGoalService(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ---------------------------------------------------------
        // CREATE
        // ---------------------------------------------------------

        public async Task<SkillDevelopmentGoalResponseDto> CreateAsync(
            CreateSkillDevelopmentGoalRequestDto request,
            int userId)
        {
            SkillSuggestion? suggestion = null;

            // ---------------------------------------------------------
            // SKILL SUGGESTION KONTROLÜ
            // ---------------------------------------------------------

            if (request.SkillSuggestionId.HasValue)
            {
                suggestion = await _unitOfWork.SkillSuggestions
                    .FirstOrDefaultAsync(x =>
                        x.Id == request.SkillSuggestionId.Value &&
                        x.Cv.UserId == userId);

                if (suggestion == null)
                {
                    throw new UnauthorizedAccessException(
                        "Bu skill önerisine erişim yetkiniz yok.");
                }
            }

            // ---------------------------------------------------------
            // DUPLICATE GOAL KONTROLÜ
            // ---------------------------------------------------------

            var existingGoal =
                await _unitOfWork.SkillDevelopmentGoals
                    .FirstOrDefaultAsync(x =>
                        x.UserId == userId &&
                        x.SkillName == request.SkillName &&
                        x.Status == GoalStatus.Active);

            if (existingGoal != null)
            {
                throw new InvalidOperationException(
                    "Bu skill için zaten aktif bir geliştirme hedefiniz bulunuyor.");
            }

            // ---------------------------------------------------------
            // RECOMMENDED TARGET LEVEL
            // ---------------------------------------------------------

            SkillLevel recommendedTargetLevel;

            if (suggestion != null)
            {
                // AI önerisinden gelen gerçek önerilen seviyeyi kullan.
                recommendedTargetLevel =
                    suggestion.RecommendedTargetLevel;
            }
            else
            {
                // Kullanıcı kendi skill'ini oluşturuyorsa
                // AI önerisi olmadığı için seçtiği hedef seviyeyi
                // başlangıçta önerilen seviye olarak kabul ediyoruz.
                recommendedTargetLevel =
                    request.TargetLevel;
            }

            // ---------------------------------------------------------
            // CREATE ENTITY
            // ---------------------------------------------------------

            var goal = new SkillDevelopmentGoal
            {
                UserId = userId,

                SkillSuggestionId =
                    request.SkillSuggestionId,

                SkillName =
                    request.SkillName,

                CurrentLevel =
                    request.CurrentLevel,

                RecommendedTargetLevel =
                    recommendedTargetLevel,

                TargetLevel =
                    request.TargetLevel,

                PreferredDuration =
                    request.PreferredDuration,

                WantsPaidCourse =
                    request.WantsPaidCourse,

                WantsCertificate =
                    request.WantsCertificate,

                Purpose =
                    request.Purpose,

                Status =
                    GoalStatus.Active
            };

            await _unitOfWork.SkillDevelopmentGoals
                .AddAsync(goal);

            await _unitOfWork.SaveChangesAsync();

            return MapToDto(goal);
        }


        // ---------------------------------------------------------
        // GET ALL
        // ---------------------------------------------------------

        public async Task<IEnumerable<SkillDevelopmentGoalResponseDto>>
            GetAllAsync(int userId)
        {
            var goals =
                await _unitOfWork.SkillDevelopmentGoals
                    .FindAsync(x => x.UserId == userId);

            return goals.Select(MapToDto);
        }


        // ---------------------------------------------------------
        // GET BY ID
        // ---------------------------------------------------------

        public async Task<SkillDevelopmentGoalResponseDto?>
            GetByIdAsync(
                int goalId,
                int userId)
        {
            var goal =
                await _unitOfWork.SkillDevelopmentGoals
                    .FirstOrDefaultAsync(x =>
                        x.Id == goalId &&
                        x.UserId == userId);

            if (goal == null)
                return null;

            return MapToDto(goal);
        }


        // ---------------------------------------------------------
        // UPDATE
        // ---------------------------------------------------------

        public async Task<SkillDevelopmentGoalResponseDto?>
            UpdateAsync(
                int goalId,
                UpdateSkillDevelopmentGoalRequestDto request,
                int userId)
        {
            var goal =
                await _unitOfWork.SkillDevelopmentGoals
                    .FirstOrDefaultAsync(x =>
                        x.Id == goalId &&
                        x.UserId == userId);

            if (goal == null)
                return null;

            // ---------------------------------------------------------
            // UPDATE
            // ---------------------------------------------------------

            goal.CurrentLevel =
                request.CurrentLevel;

            goal.TargetLevel =
                request.TargetLevel;

            goal.PreferredDuration =
                request.PreferredDuration;

            goal.WantsPaidCourse =
                request.WantsPaidCourse;

            goal.WantsCertificate =
                request.WantsCertificate;

            goal.Purpose =
                request.Purpose;

            // Status burada değiştirilmez.
            // Status için ileride ayrı Complete / Cancel endpoint'i
            // oluşturabiliriz.

            _unitOfWork.SkillDevelopmentGoals
                .Update(goal);

            await _unitOfWork.SaveChangesAsync();

            return MapToDto(goal);
        }


        // ---------------------------------------------------------
        // DELETE
        // ---------------------------------------------------------

        public async Task<bool> DeleteAsync(
            int goalId,
            int userId)
        {
            var goal =
                await _unitOfWork.SkillDevelopmentGoals
                    .FirstOrDefaultAsync(x =>
                        x.Id == goalId &&
                        x.UserId == userId);

            if (goal == null)
                return false;

            _unitOfWork.SkillDevelopmentGoals
                .Delete(goal);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }


        // ---------------------------------------------------------
        // MAPPING
        // ---------------------------------------------------------

        private static SkillDevelopmentGoalResponseDto
            MapToDto(SkillDevelopmentGoal goal)
        {
            return new SkillDevelopmentGoalResponseDto
            {
                Id =
                    goal.Id,

                UserId =
                    goal.UserId,

                SkillSuggestionId =
                    goal.SkillSuggestionId,

                SkillName =
                    goal.SkillName,

                CurrentLevel =
                    goal.CurrentLevel,

                RecommendedTargetLevel =
                    goal.RecommendedTargetLevel,

                TargetLevel =
                    goal.TargetLevel,

                PreferredDuration =
                    goal.PreferredDuration,

                WantsPaidCourse =
                    goal.WantsPaidCourse,

                WantsCertificate =
                    goal.WantsCertificate,

                Purpose =
                    goal.Purpose,

                Status =
                    goal.Status,

                CreatedAt =
                    goal.CreatedAt
            };
        }
    }
}