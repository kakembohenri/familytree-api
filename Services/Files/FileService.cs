
using familytree_api.Database;
using familytree_api.Dtos.Family;
using familytree_api.Models;
using familytree_api.Repositories.File;

namespace familytree_api.Services.Files
{
    public class FileService(
        IFileRepository _fileRepository,
        IUnitOfWork _unitOfWork
        ) : IFileService
    {

        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

        public async Task UploadFile(FileInputDto body)
        {
            await _unitOfWork.BeginTransactionAsync();
            var file = body.File;
            int familyMemberId = body.FamilyMemberId;

            string timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmssfff"); // High precision timestamp
                string newFileName = $"{timestamp}{Path.GetExtension(file?.FileName)}";
            try
            {

                // Get file details
                string filePath = Path.Combine(_uploadPath, newFileName);

                Image newFile = new()
                {
                    Path = newFileName,
                    Type = body.Type,
                    FamilyMemberId = familyMemberId,
                    CreatedAt = DateTime.Now,
                };

                await _fileRepository.Create(newFile);

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

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

                DeleteFile(Path.Combine(_uploadPath, newFileName));

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

                DeleteFile(Path.Combine(_uploadPath, file.Path));
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
                    DeleteFile(Path.Combine(_uploadPath, file.Path));
                }

            }
            catch
            {
                throw;
            }
        }

        private static void DeleteFile(string path)
        {
            System.IO.File.Delete(path);
        }
    }
}
