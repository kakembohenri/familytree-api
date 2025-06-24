using familytree_api.Database;
using familytree_api.Dtos.Partner;
using familytree_api.Enums;
using familytree_api.Models;
using familytree_api.Repositories.FamilyMember;
using familytree_api.Repositories.Partner;
using familytree_api.Repositories.User;
using familytree_api.Services.Files;

namespace familytree_api.Services.Partner
{
    public class PartnerService(
        IPartnerRepository _partnerRepository,
        IUserRepository _userRepository,
        IFamilyMemberRepository _familyMemberRepository,
        IUnitOfWork _unitOfWork,
        IFileService _fileService
        ) : IPartnerService
    {
    
        /*
         * Assume we are creating the wife here
         * Create user as instance of wife
         * Create family member as instance of wife 
         */
        public async Task AddPartner(PartnerInputDto body)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // Create user instance for family member
                Models.User user = new()
                {
                    FirstName = body.FirstName,
                    LastName = body.LastName,
                    Role = UserRoles.Viewer.ToString(),
                    Email = body.Email,
                    CreatedAt = body.CreatedAt
                };

                await _userRepository.Create(user);

                var familyMemberUser = await _familyMemberRepository.FindByUserId(body.HusbandId);

                // Create family member instance

                Models.FamilyMember familyMember = new()
                {
                    UserId = user.Id,
                    FamilyId = (int)familyMemberUser!.FamilyId,
                    Born = body.Born,
                    Died = body.Died,
                    Gender = body.Gender,
                    ShowInTree = true,
                    CreatedAt = body.CreatedAt,
                    PlaceOfBirth = body.PlaceOfBirth,
                    Occupation = body.Occupation,
                    Bio = body.Bio,
                };

                await _familyMemberRepository.Create(familyMember);

                Models.Partner partner = new()
                {
                    HusbandId = body.HusbandId,
                    WifeId = familyMember.Id,
                    Married = body.Married,
                    Divorced = body.Divorced,
                    CreatedAt = body.CreatedAt,
                };

                await _partnerRepository.Create( partner );

                await _unitOfWork.CommitAsync();
            }
            catch
            { 
                await _unitOfWork.RollbackAsync();

                throw;
            }
        }


        /*
         * Delete all occurences husbands wife and subsequently remove wife from family member and users
         */
        public async Task DeletePartner(int husbandId)
        {
            try
            {
                // Fetch all husbands partners
                var partners = await _partnerRepository.Partners(husbandId, PartnerType.Husband);

                foreach (var partner in partners)
                {
                    var member = await _familyMemberRepository.Find(partner.WifeId);
                    await _fileService.RemoveFiles(partner.WifeId);
                    await _partnerRepository.Delete(partner);
                    await _familyMemberRepository.Delete(partner!.Wife!);
                    await _userRepository.Delete(member?.User!);
                }

            }
            catch
            {
                throw;
            }
        }

        public async Task UpdatePartnership(PartnerUpdateDto body)
        {
            try
            {
                var partnership = await _partnerRepository.Find(body.Id) ?? throw new Exception("Partnership does not exist!");

                partnership.Married = body.Married;
                partnership.Divorced = body.Divorced;

                await _partnerRepository.Update(partnership);

            }
            catch
            {
                throw;
            }
        }
    }
}
