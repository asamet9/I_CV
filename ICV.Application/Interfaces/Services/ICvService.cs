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


        Task<IEnumerable<CvResponseDto>> GetMyCvsAsync(int userId);
        Task<CvResponseDto> CreateAsync(CreateCvRequestDto request,int userId);

    }
}
