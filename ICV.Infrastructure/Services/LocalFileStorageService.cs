using ICV.Application.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;

namespace ICV.Infrastructure.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _environment;

        public LocalFileStorageService(
            IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<(string storedFileName, string storagePath)> SaveAsync(
            Stream fileStream,
            string originalFileName)
        {
            var uploadsFolder = Path.Combine(
                _environment.ContentRootPath,
                "Storage",
                "CvFiles");

            Directory.CreateDirectory(uploadsFolder);

            var extension = Path.GetExtension(originalFileName);

            var storedFileName =
                $"{Guid.NewGuid():N}{extension}";

            var fullPath = Path.Combine(
                uploadsFolder,
                storedFileName);

            await using var outputStream =
                new FileStream(
                    fullPath,
                    FileMode.Create);

            await fileStream.CopyToAsync(outputStream);

            var storagePath = Path.Combine(
                "Storage",
                "CvFiles",
                storedFileName);

            return (storedFileName, storagePath);
        }

        public Task DeleteAsync(string storagePath)
        {
            var fullPath = Path.Combine(
                _environment.ContentRootPath,
                storagePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            return Task.CompletedTask;
        }
    }
}