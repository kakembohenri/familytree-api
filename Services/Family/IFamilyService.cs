using familytree_api.Dtos.Family;

namespace familytree_api.Services.Family
{
    public interface IFamilyService
    {
        Task<TreeOutputDto?> Tree();
        Task<Models.Family> CreateFamily(FamilyInputDto body);
        Task<Models.Family?> FindFamily(int id);
        Task CreateFamilyMember(FamilyMemberInputDto body);
        Task UpdateFamilyMember(FamilyMemberUpdateDto body);
        Task DeleteFamilyMember(int familyMemberId);
      
    }
}
