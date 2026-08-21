using ICV.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Infrastructure.Services.FileStorage
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _rootPath;

        public LocalFileStorageService(IConfiguration configuration)
        {
            _rootPath = configuration["FileStorage:RootPath"]
                ?? throw new InvalidOperationException(
                    "FileStorage:RootPath configuration is missing.");
        }

        public async Task<string> SaveAsync(
            Stream fileStream,
            string fileName,
            int userId,
            CancellationToken cancellationToken = default)
        {
            var extension = Path.GetExtension(fileName);

            var storedFileName =
                $"{Guid.NewGuid():N}{extension}";

            var relativeDirectory = Path.Combine(
                "cvs",
                userId.ToString());

            var directory = Path.Combine(
                _rootPath,
                relativeDirectory);

            Directory.CreateDirectory(directory);

            var filePath = Path.Combine(
                directory,
                storedFileName);

            await using var outputStream =
                new FileStream(
                    filePath,
                    FileMode.CreateNew,
                    FileAccess.Write,
                    FileShare.None,
                    81920,
                    FileOptions.Asynchronous);

            await fileStream.CopyToAsync(
                outputStream,
                cancellationToken);

            return Path.Combine(
                relativeDirectory,
                storedFileName)
                .Replace("\\", "/");
        }

        public Task DeleteAsync(
            string storagePath,
            CancellationToken cancellationToken = default)
        {
            var filePath = Path.Combine(
                _rootPath,
                storagePath.Replace(
                    "/",
                    Path.DirectorySeparatorChar.ToString()));

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return Task.CompletedTask;
        }
    }
}