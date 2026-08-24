using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ICV.Application.Interfaces.Services;

namespace ICV.Infrastructure.Services.FileParsing
{
    public class DocxTextExtractor : IDocxTextExtractor
    {
        public async Task<string> ExtractTextAsync(
            Stream fileStream,
            CancellationToken cancellationToken = default)
        {
            if (fileStream == null)
            {
                throw new ArgumentNullException(nameof(fileStream));
            }

            if (fileStream == Stream.Null)
            {
                throw new ArgumentException(
                    "DOCX file stream cannot be empty.",
                    nameof(fileStream));
            }

            if (!fileStream.CanRead)
            {
                throw new InvalidOperationException(
                    "DOCX file stream is not readable.");
            }

            cancellationToken.ThrowIfCancellationRequested();

            if (fileStream.CanSeek)
            {
                fileStream.Position = 0;
            }

            using var memoryStream = new MemoryStream();

            await fileStream.CopyToAsync(
                memoryStream,
                cancellationToken);

            memoryStream.Position = 0;

            cancellationToken.ThrowIfCancellationRequested();

            using var document =
                WordprocessingDocument.Open(
                    memoryStream,
                    false);

            var body = document.MainDocumentPart?
                .Document?
                .Body;

            if (body == null)
            {
                throw new InvalidOperationException(
                    "DOCX document does not contain a readable body.");
            }

            var paragraphs = body
                .Descendants<Paragraph>()
                .Select(p => p.InnerText?.Trim())
                .Where(text => !string.IsNullOrWhiteSpace(text));

            var text = string.Join(
                Environment.NewLine,
                paragraphs);

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new InvalidOperationException(
                    "No readable text could be extracted from the DOCX file.");
            }

            return text;
        }
    }
}