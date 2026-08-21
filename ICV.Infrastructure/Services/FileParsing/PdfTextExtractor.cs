using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Application.Interfaces.Services;
using UglyToad.PdfPig;

namespace ICV.Infrastructure.Services.FileParsing
{
    public class PdfTextExtractor : IPdfTextExtractor
    {
        public Task<string> ExtractTextAsync(
            Stream fileStream,
            CancellationToken cancellationToken = default)
        {
            if (fileStream == null)
                throw new ArgumentNullException(nameof(fileStream));

            if (!fileStream.CanRead)
                throw new InvalidOperationException("PDF file stream is not readable.");

            fileStream.Position = 0;

            using var document = PdfDocument.Open(fileStream);

            var pages = document.GetPages();

            var text = string.Join(
                Environment.NewLine,
                pages.Select(page => page.Text));

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new InvalidOperationException(
                    "No readable text could be extracted from the PDF.");
            }

            return Task.FromResult(text);
        }
    }
}