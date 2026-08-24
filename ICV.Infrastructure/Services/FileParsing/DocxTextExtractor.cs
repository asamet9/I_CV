using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Application.Interfaces.Services;

namespace ICV.Infrastructure.Services.FileParsing
{
    public class DocxTextExtractor : IDocxTextExtractor
    {
        public Task<string> ExtractTextAsync(Stream fileStream, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
