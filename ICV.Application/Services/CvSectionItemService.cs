using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Application.DTOs.CvSectionItem;
using ICV.Application.Interfaces.Services;
using ICV.Application.Interfaces.UnitOfWork;
using ICV.Domain.Entities;

namespace ICV.Application.Services
{
    public class CvSectionItemService : ICvSectionItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CvSectionItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CvSectionItemResponseDto> CreateAsync(
            CreateCvSectionItemRequestDto request,
            int userId)
        {
            // Önce Section'ın gerçekten giriş yapan kullanıcıya
            // ait bir CV içerisinde olduğunu kontrol ediyoruz.
            var section = await _unitOfWork.CvSections
                .FirstOrDefaultAsync(x =>
                    x.Id == request.CvSectionId &&
                    x.Cv.UserId == userId);

            if (section == null)
                throw new UnauthorizedAccessException(
                    "Bu Section'a erişim yetkiniz yok.");

            var item = new CvSectionItem
            {
                CvSectionId = request.CvSectionId,
                Title = request.Title,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };

            await _unitOfWork.CvSectionItems.AddAsync(item);

            await _unitOfWork.SaveChangesAsync();

            return new CvSectionItemResponseDto
            {
                Id = item.Id,
                CvSectionId = item.CvSectionId,
                Title = item.Title,
                Description = item.Description,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                CreatedAt = item.CreatedAt
            };
        }

        public async Task<IEnumerable<CvSectionItemResponseDto>> GetAllAsync(
       int cvSectionId,
       int userId)
        {
            // Önce Section'ın giriş yapan kullanıcıya ait
            // bir CV içerisinde olduğunu kontrol ediyoruz.
            var section = await _unitOfWork.CvSections
                .FirstOrDefaultAsync(x =>
                    x.Id == cvSectionId &&
                    x.Cv.UserId == userId);

            if (section == null)
                throw new UnauthorizedAccessException(
                    "Bu Section'a erişim yetkiniz yok.");

            var items = await _unitOfWork.CvSectionItems
                .FindAsync(x => x.CvSectionId == cvSectionId);

            return items
                .Select(x => new CvSectionItemResponseDto
                {
                    Id = x.Id,
                    CvSectionId = x.CvSectionId,
                    Title = x.Title,
                    Description = x.Description,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    CreatedAt = x.CreatedAt
                });
        }

        public async Task<CvSectionItemResponseDto?> GetByIdAsync(
           int itemId,
           int userId)
        {
            // Item'ı buluyoruz ve bağlı olduğu CV'nin
            // giriş yapan kullanıcıya ait olduğunu kontrol ediyoruz.
            var item = await _unitOfWork.CvSectionItems
                .FirstOrDefaultAsync(x =>
                    x.Id == itemId &&
                    x.CvSection.Cv.UserId == userId);

            if (item == null)
                return null;

            return new CvSectionItemResponseDto
            {
                Id = item.Id,
                CvSectionId = item.CvSectionId,
                Title = item.Title,
                Description = item.Description,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                CreatedAt = item.CreatedAt
            };
        }

        public async Task<CvSectionItemResponseDto?> UpdateAsync(
            int itemId,
            UpdateCvSectionItemRequestDto request,
            int userId)
        {
            // Item'ı buluyoruz ve bağlı olduğu CV'nin
            // giriş yapan kullanıcıya ait olduğunu kontrol ediyoruz.
            var item = await _unitOfWork.CvSectionItems
                .FirstOrDefaultAsync(x =>
                    x.Id == itemId &&
                    x.CvSection.Cv.UserId == userId);

            if (item == null)
                return null;

            item.Title = request.Title;
            item.Description = request.Description;
            item.StartDate = request.StartDate;
            item.EndDate = request.EndDate;

            _unitOfWork.CvSectionItems.Update(item);

            await _unitOfWork.SaveChangesAsync();

            return new CvSectionItemResponseDto
            {
                Id = item.Id,
                CvSectionId = item.CvSectionId,
                Title = item.Title,
                Description = item.Description,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                CreatedAt = item.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(
            int itemId,
            int userId)
        {
            var item = await _unitOfWork.CvSectionItems
                .FirstOrDefaultAsync(x =>
                    x.Id == itemId &&
                    x.CvSection.Cv.UserId == userId);

            if (item == null)
                return false;

            _unitOfWork.CvSectionItems.Delete(item);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}