using ICV.Application.DTOs.Cv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Application.DTOs.Cv;


namespace ICV.Application.Interfaces.Services
{
    public interface ICvService
    {

        Task<bool> DeleteAsync(int cvId, int userId);
        Task<CvResponseDto?> UpdateAsync(int cvId,UpdateCvRequestDto request,int userId);
           
        Task<CvResponseDto?> GetByIdAsync(int cvId, int userId);
        Task<IEnumerable<CvResponseDto>> GetMyCvsAsync(int userId);
        Task<CvResponseDto> CreateAsync(CreateCvRequestDto request,int userId);

    }
}
