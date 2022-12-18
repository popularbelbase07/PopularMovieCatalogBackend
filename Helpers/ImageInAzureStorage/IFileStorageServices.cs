namespace PopularMovieCatalogBackend.Helpers.ImageInAzureStorage
{
    public interface IFileStorageServices
    {
        Task DeleteFile(string containerName, string fileRoute);

        Task<string> SaveFiles(string containerName, IFormFile file);

        Task<string> EditFile(string containerName, IFormFile file, string fileRoute);

    }
}
