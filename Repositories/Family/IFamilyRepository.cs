namespace familytree_api.Repositories.Family
{
    public interface IFamilyRepository
    {
        Task<Models.Family> Create(Models.Family body);
        Task<Models.Family> Update(Models.Family body);
        Task<Models.Family?> FindById(int id);
    }
}
