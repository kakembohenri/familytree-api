namespace familytree_api.Repositories.File
{
    public interface IFileRepository
    {
        Task<Models.Image> Create(Models.Image body);
        Task<List<Models.Image>> List(int fmailyMemberId);
        Task  Delete(int id);
        Task  DeleteAll(int familyMemberId);
        Task<Models.Image?> Find(int id);
    }
}
