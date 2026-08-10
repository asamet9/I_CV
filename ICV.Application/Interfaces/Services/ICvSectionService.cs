using ICV.Application.DTOs.CvSection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Application.Interfaces.Services
{
    public interface ICvSectionService
    {

        Task<bool> DeleteAsync(
    int sectionId,
    int userId);

        Task<CvSectionResponseDto?> UpdateAsync(
    int sectionId,
    UpdateCvSectionRequestDto request,
    int userId);

        Task<CvSectionResponseDto?> GetByIdAsync(
    int sectionId,
    int userId);

        Task<IEnumerable<CvSectionResponseDto>> GetAllAsync(
    int cvId,
    int userId);

        Task<CvSectionResponseDto> CreateAsync(
          int cvId,
          CreateCvSectionRequestDto request,
          int userId);

    }

}
