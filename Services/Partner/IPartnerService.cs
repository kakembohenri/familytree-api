using familytree_api.Dtos.Partner;

namespace familytree_api.Services.Partner
{
    public interface IPartnerService
    {
        //Task FetchPartner(int partnerId);
        Task AddPartner(PartnerInputDto body);
        Task UpdatePartnership(PartnerUpdateDto body);
        Task DeletePartner(int husbandId);
       
    }
}
