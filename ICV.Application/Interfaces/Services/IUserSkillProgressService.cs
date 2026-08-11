using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Application.DTOs.UserSkillProgress;

namespace ICV.Application.Interfaces.Services
{
    public interface IUserSkillProgressService
    {
        Task<UserSkillProgressResponseDto> CreateAsync(
            CreateUserSkillProgressRequestDto request,
            int userId);

        Task<IEnumerable<UserSkillProgressResponseDto>> GetAllAsync(
            int userId);

        Task<UserSkillProgressResponseDto?> GetByIdAsync(
            int progressId,
            int userId);

        Task<UserSkillProgressResponseDto?> UpdateAsync(
            int progressId,
            UpdateUserSkillProgressRequestDto request,
            int userId);

        Task<bool> DeleteAsync(
            int progressId,
            int userId);
    }
}
