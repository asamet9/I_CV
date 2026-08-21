using ICV.Application.DTOs.CvFile;

namespace ICV.Application.Interfaces.Services
{
    public interface ICvFileService
    {
        Task<CvFileResponseDto> UploadAsync(
            int cvId,
            Stream fileStream,
            string originalFileName,
            string contentType,
            long fileSize,
            int userId);

        Task<CvFileResponseDto?> GetByCvIdAsync(
            int cvId,
            int userId);

        Task<bool> DeleteAsync(
            int cvId,
            int userId);
    }
}