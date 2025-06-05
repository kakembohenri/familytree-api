namespace familytree_api.Repositories.FamilyMember
{
    public interface IFamilyMemberRepository
    {
        Task<Models.FamilyMember> Create(Models.FamilyMember body);
        Task<Models.FamilyMember> Update(Models.FamilyMember body);
        Task<Models.FamilyMember?> Find(int id);
        Task<List<Models.FamilyMember>> GetChildren(int? fatherId, int? motherId);
        Task<Models.FamilyMember?> FindByUserId(int userId);
        Task Delete(Models.FamilyMember body);
        Task<Models.FamilyMember?> ShowInTree(int familyId);
    }
}
