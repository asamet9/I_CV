using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICV.Application.DTOs.CvSectionItem;

namespace ICV.Application.Interfaces.Services
{
    public interface ICvSectionItemService
    {
        Task<CvSectionItemResponseDto> CreateAsync(
            CreateCvSectionItemRequestDto request,
            int userId);

        Task<IEnumerable<CvSectionItemResponseDto>> GetAllAsync(
            int cvSectionId,
            int userId);

        Task<CvSectionItemResponseDto?> GetByIdAsync(
            int itemId,
            int userId);

        Task<CvSectionItemResponseDto?> UpdateAsync(
            int itemId,
            UpdateCvSectionItemRequestDto request,
            int userId);

        Task<bool> DeleteAsync(
            int itemId,
            int userId);
    }
}