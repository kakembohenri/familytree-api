using familytree_api.Dtos.Family;

namespace familytree_api.Services.Files
{
    public interface IFileService
    {
        Task UploadFile(FileInputDto body);
        Task RemoveFile(int id);
        Task RemoveFiles(int familyMemberId);
    }
}
