using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Application.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveAsync(
            Stream fileStream,
            string fileName,
            int userId,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            string storagePath,
            CancellationToken cancellationToken = default);
    }
}