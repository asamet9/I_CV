using ICV.Application.DTOs.Cv;
using ICV.Application.Interfaces.Services;
using ICV.Application.Interfaces.UnitOfWork;
using ICV.Domain.Entities;

namespace ICV.Application.Services
{
    public class CvService : ICvService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CvService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<bool> DeleteAsync(int cvId, int userId)
        {
            var cv = await _unitOfWork.Cvs
                .FirstOrDefaultAsync(x =>
                    x.Id == cvId &&
                    x.UserId == userId);

            if (cv == null)
                return false;

            _unitOfWork.Cvs.Delete(cv);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<CvResponseDto?> UpdateAsync(
    int cvId,
    UpdateCvRequestDto request,
    int userId)
        {
            var cv = await _unitOfWork.Cvs
                .FirstOrDefaultAsync(x =>
                    x.Id == cvId &&
                    x.UserId == userId);

            if (cv == null)
                return null;

            cv.ProfessionId = request.ProfessionId;
            cv.Title = request.Title;
            cv.Summary = request.Summary;

            _unitOfWork.Cvs.Update(cv);

            await _unitOfWork.SaveChangesAsync();

            return new CvResponseDto
            {
                Id = cv.Id,
                UserId = cv.UserId,
                ProfessionId = cv.ProfessionId,
                Title = cv.Title,
                Summary = cv.Summary,
                Source = (int)cv.Source,
                CreatedAt = cv.CreatedAt
            };
        }

        public async Task<CvResponseDto?> GetByIdAsync(int cvId, int userId)
        {
            var cv = await _unitOfWork.Cvs
                .FirstOrDefaultAsync(x =>
                    x.Id == cvId &&
                    x.UserId == userId);

            if (cv == null)
                return null;

            return new CvResponseDto
            {
                Id = cv.Id,
                UserId = cv.UserId,
                ProfessionId = cv.ProfessionId,
                Title = cv.Title,
                Summary = cv.Summary,
                Source = (int)cv.Source,
                CreatedAt = cv.CreatedAt
            };
        }

        public async Task<IEnumerable<CvResponseDto>> GetMyCvsAsync(int userId)
        {
            var cvs = await _unitOfWork.Cvs
                .FindAsync(x => x.UserId == userId);

            return cvs.Select(cv => new CvResponseDto
            {
                Id = cv.Id,
                UserId = cv.UserId,
                ProfessionId = cv.ProfessionId,
                Title = cv.Title,
                Summary = cv.Summary,
                Source = (int)cv.Source,
                CreatedAt = cv.CreatedAt
            });
        }

        public async Task<CvResponseDto> CreateAsync(
            CreateCvRequestDto request,
            int userId)
        {
            // Yeni CV nesnesi oluşturuyoruz
            var cv = new Cv
            {
                UserId = userId,
                ProfessionId = request.ProfessionId,
                Title = request.Title,
                Summary = request.Summary
            };

            // CV'yi veritabanına ekle
            await _unitOfWork.Cvs.AddAsync(cv);

            // Değişiklikleri veritabanına kaydet
            await _unitOfWork.SaveChangesAsync();

            // Entity'yi Response DTO'ya dönüştür
            return new CvResponseDto
            {
                Id = cv.Id,
                UserId = cv.UserId,
                ProfessionId = cv.ProfessionId,
                Title = cv.Title,
                Summary = cv.Summary,
                Source = (int)cv.Source,
                CreatedAt = cv.CreatedAt
            };


        }
    }
}