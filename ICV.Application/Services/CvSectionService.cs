using ICV.Application.DTOs.CvSection;
using ICV.Application.Interfaces.Services;
using ICV.Application.Interfaces.UnitOfWork;
using ICV.Domain.Entities;
using ICV.Domain.Enums;

namespace ICV.Application.Services
{
    public class CvSectionService : ICvSectionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CvSectionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> DeleteAsync(
    int sectionId,
    int userId)
        {
            // Section'ın bu kullanıcıya ait olduğunu kontrol ediyoruz.
            var section = await _unitOfWork.CvSections
                .FirstOrDefaultAsync(x =>
                    x.Id == sectionId &&
                    x.Cv.UserId == userId);

            if (section == null)
                return false;

            _unitOfWork.CvSections.Delete(section);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
        public async Task<CvSectionResponseDto?> UpdateAsync(
    int sectionId,
    UpdateCvSectionRequestDto request,
    int userId)
        {
            // Section'ı buluyoruz ve bağlı olduğu CV'nin
            // giriş yapan kullanıcıya ait olduğunu kontrol ediyoruz.
            var section = await _unitOfWork.CvSections
                .FirstOrDefaultAsync(x =>
                    x.Id == sectionId &&
                    x.Cv.UserId == userId);

            if (section == null)
                return null;

            section.Type = (CvSectionType)request.Type;
            section.OrderIndex = request.OrderIndex;

            _unitOfWork.CvSections.Update(section);

            await _unitOfWork.SaveChangesAsync();

            return new CvSectionResponseDto
            {
                Id = section.Id,
                CvId = section.CvId,
                Type = (int)section.Type,
                OrderIndex = section.OrderIndex,
                CreatedAt = section.CreatedAt
            };
        }


        public async Task<CvSectionResponseDto?> GetByIdAsync(
    int sectionId,
    int userId)
        {
            // Section'ı buluyoruz ve bağlı olduğu CV'nin
            // giriş yapan kullanıcıya ait olduğunu kontrol ediyoruz.
            var section = await _unitOfWork.CvSections
                .FirstOrDefaultAsync(x =>
                    x.Id == sectionId &&
                    x.Cv.UserId == userId);

            if (section == null)
                return null;

            return new CvSectionResponseDto
            {
                Id = section.Id,
                CvId = section.CvId,
                Type = (int)section.Type,
                OrderIndex = section.OrderIndex,
                CreatedAt = section.CreatedAt
            };
        }

        public async Task<IEnumerable<CvSectionResponseDto>> GetAllAsync(
    int cvId,
    int userId)
        {
            // Önce CV'nin bu kullanıcıya ait olduğunu kontrol ediyoruz.
            var cv = await _unitOfWork.Cvs
                .FirstOrDefaultAsync(x =>
                    x.Id == cvId &&
                    x.UserId == userId);

            if (cv == null)
                throw new UnauthorizedAccessException(
                    "Bu CV'ye erişim yetkiniz yok.");

            var sections = await _unitOfWork.CvSections
                .FindAsync(x => x.CvId == cvId);

            return sections
                .OrderBy(x => x.OrderIndex)
                .Select(x => new CvSectionResponseDto
                {
                    Id = x.Id,
                    CvId = x.CvId,
                    Type = (int)x.Type,
                    OrderIndex = x.OrderIndex,
                    CreatedAt = x.CreatedAt
                });
        }


        public async Task<CvSectionResponseDto> CreateAsync(
            int cvId,
            CreateCvSectionRequestDto request,
            int userId)
        {
            // Önce CV'nin bu kullanıcıya ait olup olmadığını kontrol ediyoruz.
            var cv = await _unitOfWork.Cvs
                .FirstOrDefaultAsync(x =>
                    x.Id == cvId &&
                    x.UserId == userId);

            if (cv == null)
                throw new UnauthorizedAccessException(
                    "Bu CV'ye erişim yetkiniz yok.");

            var section = new CvSection
            {
                CvId = cvId,
                Type = (CvSectionType)request.Type,
                OrderIndex = request.OrderIndex
            };

            await _unitOfWork.CvSections.AddAsync(section);

            await _unitOfWork.SaveChangesAsync();

            return new CvSectionResponseDto
            {
                Id = section.Id,
                CvId = section.CvId,
                Type = (int)section.Type,
                OrderIndex = section.OrderIndex,
                CreatedAt = section.CreatedAt
            };
        }
    }
}