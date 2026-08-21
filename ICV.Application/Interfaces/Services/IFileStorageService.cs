namespace ICV.Application.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task<(string storedFileName, string storagePath)> SaveAsync(
            Stream fileStream,
            string originalFileName);

        Task DeleteAsync(string storagePath);
    }
}