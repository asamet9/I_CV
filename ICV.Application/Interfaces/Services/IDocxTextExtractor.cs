using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Application.Interfaces.Services
{
    public interface IDocxTextExtractor
    {
        Task<string> ExtractTextAsync(
            Stream fileStream,
            CancellationToken cancellationToken = default);
    }
}