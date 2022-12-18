using Azure.Storage.Blobs;
using Microsoft.IdentityModel.Tokens;

namespace PopularMovieCatalogBackend.Helpers.ImageInAzureStorage
{
    public class AzureStorageServices : IFileStorageServices
    {
        private string connectionString;
        public AzureStorageServices( IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureStorageConnection");
        }

        public async Task DeleteFile(string containerName, string fileRoute)
        {
            if (string.IsNullOrEmpty(fileRoute))
            {
                return;
            }
            var client = new BlobContainerClient(connectionString, containerName);
            await client.CreateIfNotExistsAsync();
            var fileName = Path.GetFileName(fileRoute); 
            var blob = client.GetBlobClient(fileName);
            await blob.DeleteAsync();

        }

        public async Task<string> EditFile(string containerName, IFormFile file, string fileRoute)
        {
            // there is a utility method that allow to delete the file and create new one
            await DeleteFile(containerName, fileRoute);
            return await SaveFiles(containerName, file);

        }

        public async Task<string> SaveFiles(string containerName, IFormFile file)
        {
            var client = new BlobContainerClient(connectionString, containerName);
            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var blob = client.GetBlobClient(fileName);
            await blob.UploadAsync(file.OpenReadStream());
            return blob.Uri.ToString(); 
        }
    }
}
