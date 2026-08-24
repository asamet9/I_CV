using ICV.Application.Interfaces.Services;
using UglyToad.PdfPig;
using System.Text;

namespace ICV.Infrastructure.Services.FileParsing
{
    public class PdfTextExtractor : IPdfTextExtractor
    {
        public async Task<string> ExtractTextAsync(
            Stream fileStream,
            CancellationToken cancellationToken = default)
        {
            if (fileStream == null)
            {
                throw new ArgumentNullException(nameof(fileStream));
            }

            if (!fileStream.CanRead)
            {
                throw new InvalidOperationException(
                    "PDF file stream cannot be read.");
            }

            if (fileStream.CanSeek)
            {
                fileStream.Position = 0;
            }

            var text = new StringBuilder();

            using var document = PdfDocument.Open(fileStream);

            foreach (var page in document.GetPages())
            {
                cancellationToken.ThrowIfCancellationRequested();

                text.AppendLine(page.Text);
            }

            await Task.CompletedTask;

            return text.ToString().Trim();
        }
    }
}