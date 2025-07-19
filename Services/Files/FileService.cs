
using Azure.Storage.Blobs;
using familytree_api.Database;
using familytree_api.Dtos.Family;
using familytree_api.Models;
using familytree_api.Repositories.File;

namespace familytree_api.Services.Files
{
    public class FileService : IFileService
    {

        private readonly IFileRepository _fileRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IWebHostEnvironment _env;
        private readonly string _containerName = "familytree";

        public FileService(
            IFileRepository fileRepository,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment env,
            IConfiguration configuration) // Inject IConfiguration
        {
            _fileRepository = fileRepository;
            _unitOfWork = unitOfWork;
            _env = env;

            var connectionString = configuration["AzureBlobStorage"]; // From appsettings.json
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task UploadFile(FileInputDto body)
        {
            await _unitOfWork.BeginTransactionAsync();
            var file = body.File;
            int familyMemberId = body.FamilyMemberId;


            string timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmssfff"); // High precision timestamp
            string newFileName = $"{timestamp}{Path.GetExtension(file?.FileName)}";
            try
            {
                if (_env.IsDevelopment())
                {
                    string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

                    // Get file details
                    string filePath = Path.Combine(_uploadPath, newFileName);

                    // Save the file
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                }
                else
                {
                    var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                    await containerClient.CreateIfNotExistsAsync();

                    var blobClient = containerClient.GetBlobClient(newFileName);

                    using (var stream = file.OpenReadStream())
                    {
                        await blobClient.UploadAsync(stream, overwrite: true);
                    }
                }

                Image newFile = new()
                {
                    Path = newFileName,
                    Type = body.Type,
                    FamilyMemberId = familyMemberId,
                    CreatedAt = DateTime.Now,
                };

                await _fileRepository.Create(newFile);


                // if oldfile is not zero i.e is provided, delte it
                if(body.OldAvatar != 0)
                {
                    await RemoveFile(body.OldAvatar);
                }

                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();

                DeleteFile(newFileName);

                throw;
            }
        }

        /*
         * Fetch file
         * Delete it and remove file
         */
        public async Task RemoveFile(int id)
        {
            try
            {
                var file = await _fileRepository.Find(id) ?? throw new Exception("File not found");

                await _fileRepository.Delete(id);

                DeleteFile(file.Path);
            }
            catch
            {
                throw;
            }
        }


        /*
         * Fetch files
         * Delete them
         */
        public async Task RemoveFiles(int familyMemberId)
        {
            try
            {
                var files = await _fileRepository.List(familyMemberId);

                foreach(var file in files)
                {
                    await _fileRepository.Delete(file.Id);
                    DeleteFile(file.Path);
                }

            }
            catch
            {
                throw;
            }
        }

        private async void DeleteFile(string fileName)
        {
            if (_env.IsDevelopment())
            {
                System.IO.File.Delete(fileName);
            }
            else
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                var blobClient = containerClient.GetBlobClient(fileName);
                await blobClient.DeleteIfExistsAsync();
            }
        }
    }
}
