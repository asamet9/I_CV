using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICV.Application.DTOs.Profession;

namespace ICV.Application.Interfaces.Services
{
    public interface IProfessionService
    {
        Task<ProfessionResponseDto> CreateAsync(
            CreateProfessionRequestDto request);

        Task<IEnumerable<ProfessionResponseDto>> GetAllAsync();

        Task<ProfessionResponseDto?> GetByIdAsync(
            int professionId);

        Task<ProfessionResponseDto?> UpdateAsync(
            int professionId,
            UpdateProfessionRequestDto request);

        Task<bool> DeleteAsync(
            int professionId);
    }
}