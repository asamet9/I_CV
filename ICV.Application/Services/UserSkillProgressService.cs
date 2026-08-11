using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Application.DTOs.UserSkillProgress;
using ICV.Application.Interfaces.Services;
using ICV.Application.Interfaces.UnitOfWork;
using ICV.Domain.Entities;

namespace ICV.Application.Services
{
    public class UserSkillProgressService : IUserSkillProgressService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserSkillProgressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserSkillProgressResponseDto> CreateAsync(
            CreateUserSkillProgressRequestDto request,
            int userId)
        {
            // SkillSuggestion'ın gerçekten bu kullanıcıya ait olup
            // olmadığını kontrol ediyoruz.
            var skillSuggestion = await _unitOfWork.SkillSuggestions
                .FirstOrDefaultAsync(x =>
                    x.Id == request.SkillSuggestionId &&
                    x.Cv.UserId == userId);

            if (skillSuggestion == null)
                throw new UnauthorizedAccessException(
                    "Bu skill önerisine erişim yetkiniz yok.");

            // Aynı skill önerisi için daha önce progress kaydı
            // oluşturulmuş mu kontrol ediyoruz.
            var existingProgress = await _unitOfWork.UserSkillProgresses
                .FirstOrDefaultAsync(x =>
                    x.SkillSuggestionId == request.SkillSuggestionId &&
                    x.UserId == userId);

            if (existingProgress != null)
                throw new InvalidOperationException(
                    "Bu skill önerisi için zaten bir ilerleme kaydı mevcut.");

            var progress = new UserSkillProgress
            {
                UserId = userId,
                SkillSuggestionId = request.SkillSuggestionId,
                Status = request.Status,
                LastCheckedAt = request.LastCheckedAt,
                CheckIntervalDays = request.CheckIntervalDays
            };

            await _unitOfWork.UserSkillProgresses
                .AddAsync(progress);

            await _unitOfWork.SaveChangesAsync();

            return new UserSkillProgressResponseDto
            {
                Id = progress.Id,
                UserId = progress.UserId,
                SkillSuggestionId = progress.SkillSuggestionId,
                Status = progress.Status,
                LastCheckedAt = progress.LastCheckedAt,
                CheckIntervalDays = progress.CheckIntervalDays,
                CreatedAt = progress.CreatedAt
            };
        }

        public async Task<IEnumerable<UserSkillProgressResponseDto>> GetAllAsync(
            int userId)
        {
            var progresses = await _unitOfWork.UserSkillProgresses
                .FindAsync(x => x.UserId == userId);

            return progresses.Select(x => new UserSkillProgressResponseDto
            {
                Id = x.Id,
                UserId = x.UserId,
                SkillSuggestionId = x.SkillSuggestionId,
                Status = x.Status,
                LastCheckedAt = x.LastCheckedAt,
                CheckIntervalDays = x.CheckIntervalDays,
                CreatedAt = x.CreatedAt
            });
        }

        public async Task<UserSkillProgressResponseDto?> GetByIdAsync(
            int progressId,
            int userId)
        {
            var progress = await _unitOfWork.UserSkillProgresses
                .FirstOrDefaultAsync(x =>
                    x.Id == progressId &&
                    x.UserId == userId);

            if (progress == null)
                return null;

            return new UserSkillProgressResponseDto
            {
                Id = progress.Id,
                UserId = progress.UserId,
                SkillSuggestionId = progress.SkillSuggestionId,
                Status = progress.Status,
                LastCheckedAt = progress.LastCheckedAt,
                CheckIntervalDays = progress.CheckIntervalDays,
                CreatedAt = progress.CreatedAt
            };
        }

        public async Task<UserSkillProgressResponseDto?> UpdateAsync(
            int progressId,
            UpdateUserSkillProgressRequestDto request,
            int userId)
        {
            var progress = await _unitOfWork.UserSkillProgresses
                .FirstOrDefaultAsync(x =>
                    x.Id == progressId &&
                    x.UserId == userId);

            if (progress == null)
                return null;

            progress.Status = request.Status;
            progress.LastCheckedAt = request.LastCheckedAt;
            progress.CheckIntervalDays = request.CheckIntervalDays;

            _unitOfWork.UserSkillProgresses.Update(progress);

            await _unitOfWork.SaveChangesAsync();

            return new UserSkillProgressResponseDto
            {
                Id = progress.Id,
                UserId = progress.UserId,
                SkillSuggestionId = progress.SkillSuggestionId,
                Status = progress.Status,
                LastCheckedAt = progress.LastCheckedAt,
                CheckIntervalDays = progress.CheckIntervalDays,
                CreatedAt = progress.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(
            int progressId,
            int userId)
        {
            var progress = await _unitOfWork.UserSkillProgresses
                .FirstOrDefaultAsync(x =>
                    x.Id == progressId &&
                    x.UserId == userId);

            if (progress == null)
                return false;

            _unitOfWork.UserSkillProgresses.Delete(progress);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}

