using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICV.Application.DTOs.SkillDevelopmentGoal;
using ICV.Application.Interfaces.Services;
using ICV.Application.Interfaces.UnitOfWork;
using ICV.Domain.Entities;

namespace ICV.Application.Services
{
    /// <summary>
    /// Kullanıcının skill geliştirme hedeflerini yöneten servistir.
    /// </summary>
    public class SkillDevelopmentGoalService : ISkillDevelopmentGoalService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SkillDevelopmentGoalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ---------------------------------------------------------
        // CREATE
        // ---------------------------------------------------------

        /// <summary>
        /// Kullanıcı için yeni bir skill geliştirme hedefi oluşturur.
        /// </summary>
        public async Task<SkillDevelopmentGoalResponseDto> CreateAsync(
            CreateSkillDevelopmentGoalRequestDto request,
            int userId)
        {
            // Eğer hedef bir AI önerisinden oluşturuluyorsa
            // önerinin gerçekten bu kullanıcıya ait olup olmadığını
            // kontrol ediyoruz.
            if (request.SkillSuggestionId.HasValue)
            {
                var suggestion = await _unitOfWork.SkillSuggestions
                    .FirstOrDefaultAsync(x =>
                        x.Id == request.SkillSuggestionId.Value &&
                        x.Cv.UserId == userId);

                if (suggestion == null)
                {
                    throw new UnauthorizedAccessException(
                        "Bu skill önerisine erişim yetkiniz yok.");
                }
            }

            // Aynı kullanıcı aynı skill için zaten aktif bir hedef
            // oluşturmuşsa ikinci kez oluşturmasını engelliyoruz.
            var existingGoal = await _unitOfWork.SkillDevelopmentGoals
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.SkillName == request.SkillName &&
                    x.Status == Domain.Enums.GoalStatus.Active);

            if (existingGoal != null)
            {
                throw new InvalidOperationException(
                    "Bu skill için zaten aktif bir geliştirme hedefiniz bulunuyor.");
            }

            // Yeni hedef oluşturuyoruz.
            var goal = new SkillDevelopmentGoal
            {
                UserId = userId,
                SkillSuggestionId = request.SkillSuggestionId,
                SkillName = request.SkillName,
                CurrentLevel = request.CurrentLevel,
                TargetLevel = request.TargetLevel,
                TargetDate = request.TargetDate,
                WeeklyHours = request.WeeklyHours,
                Purpose = request.Purpose,
                Status = Domain.Enums.GoalStatus.Active
            };

            // Hedefi veritabanına eklenmek üzere repository'ye gönderiyoruz.
            await _unitOfWork.SkillDevelopmentGoals
                .AddAsync(goal);

            // Değişiklikleri kaydediyoruz.
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(goal);
        }

        // ---------------------------------------------------------
        // GET ALL
        // ---------------------------------------------------------

        /// <summary>
        /// Kullanıcının kendi skill geliştirme hedeflerini getirir.
        /// </summary>
        public async Task<IEnumerable<SkillDevelopmentGoalResponseDto>> GetAllAsync(
            int userId)
        {
            // Sadece giriş yapan kullanıcıya ait hedefleri getiriyoruz.
            var goals = await _unitOfWork.SkillDevelopmentGoals
                .FindAsync(x => x.UserId == userId);

            return goals.Select(MapToDto);
        }

        // ---------------------------------------------------------
        // GET BY ID
        // ---------------------------------------------------------

        /// <summary>
        /// Kullanıcının kendi hedeflerinden belirli bir tanesini getirir.
        /// </summary>
        public async Task<SkillDevelopmentGoalResponseDto?> GetByIdAsync(
            int goalId,
            int userId)
        {
            // Hem hedef ID'sini hem de kullanıcı ID'sini kontrol ediyoruz.
            var goal = await _unitOfWork.SkillDevelopmentGoals
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

        /// <summary>
        /// Kullanıcının kendi skill geliştirme hedefini günceller.
        /// </summary>
        public async Task<SkillDevelopmentGoalResponseDto?> UpdateAsync(
            int goalId,
            UpdateSkillDevelopmentGoalRequestDto request,
            int userId)
        {
            // Hedefin giriş yapan kullanıcıya ait olduğunu kontrol ediyoruz.
            var goal = await _unitOfWork.SkillDevelopmentGoals
                .FirstOrDefaultAsync(x =>
                    x.Id == goalId &&
                    x.UserId == userId);

            if (goal == null)
                return null;

            // Güncellenebilir alanları değiştiriyoruz.
            goal.CurrentLevel = request.CurrentLevel;
            goal.TargetLevel = request.TargetLevel;
            goal.TargetDate = request.TargetDate;
            goal.WeeklyHours = request.WeeklyHours;
            goal.Purpose = request.Purpose;
            goal.Status = request.Status;

            _unitOfWork.SkillDevelopmentGoals.Update(goal);

            await _unitOfWork.SaveChangesAsync();

            return MapToDto(goal);
        }

        // ---------------------------------------------------------
        // DELETE
        // ---------------------------------------------------------

        /// <summary>
        /// Kullanıcının kendi skill geliştirme hedefini siler.
        /// </summary>
        public async Task<bool> DeleteAsync(
            int goalId,
            int userId)
        {
            // Sadece kullanıcının kendi hedefini buluyoruz.
            var goal = await _unitOfWork.SkillDevelopmentGoals
                .FirstOrDefaultAsync(x =>
                    x.Id == goalId &&
                    x.UserId == userId);

            if (goal == null)
                return false;

            _unitOfWork.SkillDevelopmentGoals.Delete(goal);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        // ---------------------------------------------------------
        // MAPPING
        // ---------------------------------------------------------

        /// <summary>
        /// Entity'yi Response DTO'ya dönüştürür.
        /// </summary>
        private static SkillDevelopmentGoalResponseDto MapToDto(
            SkillDevelopmentGoal goal)
        {
            return new SkillDevelopmentGoalResponseDto
            {
                Id = goal.Id,
                UserId = goal.UserId,
                SkillSuggestionId = goal.SkillSuggestionId,
                SkillName = goal.SkillName,
                CurrentLevel = goal.CurrentLevel,
                TargetLevel = goal.TargetLevel,
                TargetDate = goal.TargetDate,
                WeeklyHours = goal.WeeklyHours,
                Purpose = goal.Purpose,
                Status = goal.Status,
                CreatedAt = goal.CreatedAt
            };
        }
    }
}