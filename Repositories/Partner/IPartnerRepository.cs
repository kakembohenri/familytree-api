using familytree_api.Enums;

namespace familytree_api.Repositories.Partner
{
    public interface IPartnerRepository
    {
        Task<Models.Partner> Create(Models.Partner body);
        Task<List<Models.Partner>> Partners(int familyMemberId, PartnerType partner);
        Task<Models.Partner?> Find(int partnerId);
        Task<Models.Partner> Update(Models.Partner body);
        Task Delete(Models.Partner body);
        Task DeleteAll(int familyMemberId);
    }
}
