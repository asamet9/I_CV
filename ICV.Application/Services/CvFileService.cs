using ICV.Application.DTOs.CvFile;
using ICV.Application.Interfaces.Services;
using ICV.Application.Interfaces.UnitOfWork;
using ICV.Domain.Entities;

namespace ICV.Application.Services
{
    public class CvFileService : ICvFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorageService;

        public CvFileService(
            IUnitOfWork unitOfWork,
            IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _fileStorageService = fileStorageService;
        }

        public async Task<CvFileResponseDto> UploadAsync(
            int cvId,
            Stream fileStream,
            string originalFileName,
            string contentType,
            long fileSize,
            int userId)
        {
            var cv = await _unitOfWork.Cvs
                .FirstOrDefaultAsync(x =>
                    x.Id == cvId &&
                    x.UserId == userId);

            if (cv == null)
            {
                throw new UnauthorizedAccessException(
                    "Bu CV'ye erişim yetkiniz yok.");
            }

            var existingFile = await _unitOfWork.CvFiles
                .FirstOrDefaultAsync(x => x.CvId == cvId);

            var (storedFileName, storagePath) =
                await _fileStorageService.SaveAsync(
                    fileStream,
                    originalFileName);

            if (existingFile != null)
            {
                await _fileStorageService.DeleteAsync(
                    existingFile.StoragePath);

                _unitOfWork.CvFiles.Delete(existingFile);
            }

            var cvFile = new CvFile
            {
                CvId = cvId,
                OriginalFileName = originalFileName,
                StoredFileName = storedFileName,
                StoragePath = storagePath,
                ContentType = contentType,
                FileSize = fileSize
            };

            await _unitOfWork.CvFiles.AddAsync(cvFile);

            await _unitOfWork.SaveChangesAsync();

            return MapToDto(cvFile);
        }

        public async Task<CvFileResponseDto?> GetByCvIdAsync(
            int cvId,
            int userId)
        {
            var cv = await _unitOfWork.Cvs
                .FirstOrDefaultAsync(x =>
                    x.Id == cvId &&
                    x.UserId == userId);

            if (cv == null)
            {
                throw new UnauthorizedAccessException(
                    "Bu CV'ye erişim yetkiniz yok.");
            }

            var file = await _unitOfWork.CvFiles
                .FirstOrDefaultAsync(x => x.CvId == cvId);

            if (file == null)
            {
                return null;
            }

            return MapToDto(file);
        }

        public async Task<bool> DeleteAsync(
            int cvId,
            int userId)
        {
            var cv = await _unitOfWork.Cvs
                .FirstOrDefaultAsync(x =>
                    x.Id == cvId &&
                    x.UserId == userId);

            if (cv == null)
            {
                throw new UnauthorizedAccessException(
                    "Bu CV'ye erişim yetkiniz yok.");
            }

            var file = await _unitOfWork.CvFiles
                .FirstOrDefaultAsync(x => x.CvId == cvId);

            if (file == null)
            {
                return false;
            }

            await _fileStorageService.DeleteAsync(
                file.StoragePath);

            _unitOfWork.CvFiles.Delete(file);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        private static CvFileResponseDto MapToDto(CvFile file)
        {
            return new CvFileResponseDto
            {
                Id = file.Id,
                CvId = file.CvId,
                OriginalFileName = file.OriginalFileName,
                ContentType = file.ContentType,
                FileSize = file.FileSize,
                CreatedAt = file.CreatedAt
            };
        }
    }
}