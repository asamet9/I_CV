
using ICV.Application.DTOs.UserCvAnswer;

namespace ICV.Application.Interfaces.Services
{
    public interface IUserCvAnswerService
    {
        Task<UserCvAnswerResponseDto> CreateAsync(
            int userId,
            CreateUserCvAnswerRequestDto request);

        Task<IEnumerable<UserCvAnswerResponseDto>> GetByCvIdAsync(
            int userId,
            int cvId);

        Task<UserCvAnswerResponseDto?> GetByIdAsync(
            int userId,
            int answerId);

        Task<bool> DeleteAsync(
            int userId,
            int answerId);
    }
}

