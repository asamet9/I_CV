using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ICV.Application.Interfaces.Services;

namespace ICV.Infrastructure.Services.FileParsing
{
    public class DocxTextExtractor : IDocxTextExtractor
    {
        public Task<string> ExtractTextAsync(
            Stream fileStream,
            CancellationToken cancellationToken = default)
        {
            if (fileStream == null)
                throw new ArgumentNullException(nameof(fileStream));

            if (!fileStream.CanRead)
                throw new InvalidOperationException(
                    "DOCX file stream is not readable.");

            fileStream.Position = 0;

            using var document = WordprocessingDocument.Open(
                fileStream,
                false);

            var body = document.MainDocumentPart?
                .Document?
                .Body;

            if (body == null)
            {
                throw new InvalidOperationException(
                    "The DOCX document does not contain a readable body.");
            }

            var paragraphs = body
                .Descendants<Text>()
                .Select(x => x.Text)
                .Where(x => !string.IsNullOrWhiteSpace(x));

            var text = string.Join(
                Environment.NewLine,
                paragraphs);

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new InvalidOperationException(
                    "No readable text could be extracted from the DOCX file.");
            }

            return Task.FromResult(text);
        }
    }
}