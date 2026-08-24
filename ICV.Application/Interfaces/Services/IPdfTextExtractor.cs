using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Application.Interfaces.Services
{
    public interface IPdfTextExtractor
    {
        Task<string> ExtractTextAsync(
            Stream fileStream,
            CancellationToken cancellationToken = default);
    }
}