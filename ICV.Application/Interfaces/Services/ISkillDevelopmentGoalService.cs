using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICV.Application.DTOs.SkillDevelopmentGoal;

namespace ICV.Application.Interfaces.Services
{
    /// <summary>
    /// Kullanıcının skill geliştirme hedeflerini yöneten servis sözleşmesidir.
    /// </summary>
    public interface ISkillDevelopmentGoalService
    {
        // Yeni bir skill geliştirme hedefi oluşturur.
        Task<SkillDevelopmentGoalResponseDto> CreateAsync(
            CreateSkillDevelopmentGoalRequestDto request,
            int userId);

        // Kullanıcının kendi tüm hedeflerini getirir.
        Task<IEnumerable<SkillDevelopmentGoalResponseDto>> GetAllAsync(
            int userId);

        // Kullanıcının kendi hedeflerinden belirli bir tanesini getirir.
        Task<SkillDevelopmentGoalResponseDto?> GetByIdAsync(
            int goalId,
            int userId);

        // Kullanıcının kendi hedefini günceller.
        Task<SkillDevelopmentGoalResponseDto?> UpdateAsync(
            int goalId,
            UpdateSkillDevelopmentGoalRequestDto request,
            int userId);

        // Kullanıcının kendi hedefini siler.
        Task<bool> DeleteAsync(
            int goalId,
            int userId);
    }
}