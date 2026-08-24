using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Application.DTOs.CvImport;

namespace ICV.Application.Interfaces.Services
{
    public interface ICvImportService
    {
        Task<ImportedCvDto> ImportAsync(
            ImportCvRequestDto request,
            int userId,
            CancellationToken cancellationToken = default);
    }
}