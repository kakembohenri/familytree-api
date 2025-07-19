using familytree_api.Database;
using familytree_api.Dtos.Family;
using familytree_api.Enums;
using familytree_api.Models;
using familytree_api.Repositories.Family;
using familytree_api.Repositories.FamilyMember;
using familytree_api.Repositories.File;
using familytree_api.Repositories.Partner;
using familytree_api.Repositories.User;
using familytree_api.Services.Files;
using familytree_api.Services.Partner;

namespace familytree_api.Services.Family
{
    public class FamilyService(
        IFamilyRepository _familyRepository,
        IFamilyMemberRepository _familyMemberRepository,
        IHttpContextAccessor _httpContextAccessor,
        IUserRepository _userRepository,
        IUnitOfWork _unitOfWork,
        IPartnerRepository _partnerRepository,
        IPartnerService _partnerService,
        IFileRepository _fileRepository,
        IFileService _fileService,
        IConfiguration _configuration
        ) : IFamilyService
    {
        public async Task<TreeOutputDto?> Tree()
        {
            try
            {
                var userProfile = _httpContextAccessor.HttpContext?.Items["UserProfile"] ?? throw new Exception("Unauthorized access");

                int UserId = 0;

                if (userProfile is Models.User userModel) UserId = userModel.Id;

                var user = await _userRepository.FindById(UserId) ?? throw new Exception("User not found!");

                var familyMember = await _familyMemberRepository.FindByUserId(UserId) ?? throw new Exception("Family member not found!");

                // Step 1: Get root member (starting point)
                var root = await _familyMemberRepository.ShowInTree(familyMember.FamilyId);

                if (root == null)
                    return null;

                var visited = new HashSet<int>();
                return await BuildFamilyNode(root.Id, visited);
            }
            catch
            {
                throw;
            }
        }

        public async Task<Models.Family> CreateFamily(FamilyInputDto body)
        {
            try
            {
                Models.Family input = new()
                {
                    Name = body.Name,
                    Origin = body.Origin,
                    CreatedAt = body.CreatedAt,
                };

                var family = await _familyRepository.Create(input);

                return family!;
            }
            catch
            {
                throw;
            }
        }

        /*
         * Create a user and a family member adding the family id of the currently loggedin user
         * Make sure that the loggedin user is of role admin
         * 
         */
        public async Task CreateFamilyMember(FamilyMemberInputDto body)
        {
            try{

                var userProfile = _httpContextAccessor.HttpContext?.Items["UserProfile"] ?? throw new Exception("Unauthorized access");

                int loggedInUserId = 0;

                if (userProfile is Models.User userModel) {
                    loggedInUserId = userModel.Id;

                    if (userModel.Role != UserRoles.Admin.ToString()) throw new Exception("User doesnt have permissions to perform this action!");
                }

                var loggedInUser = await _familyMemberRepository.FindByUserId(loggedInUserId) ?? throw new Exception("User doesnt exist!");

                // Create user instance for family member
                Models.User user = new()
                {
                    FirstName = body.FirstName,
                    LastName = body.LastName,
                    Role = UserRoles.Viewer.ToString(),
                    Email = body.Email,
                    PhoneNumber = body.Phone,
                    CreatedAt = DateTime.Now
                };

                var result = await _userRepository.Create(user);

                // Create family member instance

                Models.FamilyMember familyMember = new()
                {
                    UserId = user.Id,
                    FamilyId = loggedInUser.FamilyId,
                    Born = body.Born,
                    Died = body.Died,
                    FatherId = body.FatherId,
                    MotherId = body.MotherId,
                    PlaceOfBirth = body.PlaceOfBirth,
                    Occupation = body.Occupation,
                    Bio = body.Bio,
                    Gender = body.Gender,
                    ShowInTree = true,
                    CreatedAt = body.CreatedAt
                };

                await _familyMemberRepository.Create(familyMember);  
            }
            catch
            {
                throw;
            }
        }

        /*
         * Update users information both in the users table and the familymembers table
         * 
         */
        public async Task UpdateFamilyMember(FamilyMemberUpdateDto body)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {

                var userProfile = _httpContextAccessor.HttpContext?.Items["UserProfile"] ?? throw new Exception("Unauthorized access");

                int UserId = 0;

                if (userProfile is Models.User userModel) UserId = userModel.Id;

                // Update family member
                var familyMember = await _familyMemberRepository.Find(body.Id) ?? throw new Exception("Family Member doesnt exist!");

                familyMember.Born = body.Born;
                familyMember.Died = body.Died;
                familyMember.Gender = body.Gender;
                familyMember.PlaceOfBirth = body.PlaceOfBirth;
                familyMember.Occupation = body.Occupation;
                familyMember.Bio = body.Bio;

                await _familyMemberRepository.Update(familyMember);


                // Update user
                var user = await _userRepository.FindById(familyMember.UserId) ?? throw new Exception("User doesnt exist!");

                user.FirstName = body.FirstName;
                user.MiddleName = body.MiddleName;
                user.LastName = body.LastName;
                user.PhoneNumber = body.Phone;
                user.Email = body.Email;

                await _userRepository.Update(user);

                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();

                throw;
            }
        }

        public async Task<Models.Family?> FindFamily(int id)
        {
            try
            {
                return await _familyRepository.FindById(id);
            }
            catch
            {
                throw;
            }
        }
       
        /*
         * Delete images
         * Delete partners
         * Delete family members
         * Delete users
         * 
         */
        public async Task DeleteFamilyMember(int familyMemberId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var root = await _familyMemberRepository.Find(familyMemberId) ?? throw new Exception("Family Member does not exist");

                // Fetch all children of current member
                var children = await _familyMemberRepository.GetChildren(root.Id, root.MotherId);

                foreach (var child in children)
                {

                    await _fileService.RemoveFiles(child.Id);
                    await _partnerService.DeletePartner(child.Id);
                    await _familyMemberRepository.Delete(child);
                    await _userRepository.Delete(child?.User!);
                }

                await _fileService.RemoveFiles(root.Id);
                await _partnerService.DeletePartner(root.Id);
                await _familyMemberRepository.Delete(root);
                await _userRepository.Delete(root?.User!);

                await _unitOfWork.CommitAsync();

            }
            catch
            {
                await _unitOfWork.RollbackAsync();

                throw;
            }
        }

        private async Task<TreeOutputDto?> BuildFamilyNode(int memberId, HashSet<int> visited)
        {
            if (visited.Contains(memberId)) return null;
            visited.Add(memberId);

            var member = await _familyMemberRepository.Find(memberId);
            if (member == null || member.User == null) return null;

            var partners = await _partnerRepository.Partners(memberId, PartnerType.Husband);
            var partnerDtos = new List<FamilyMemberOutput>();

            var images = await _fileRepository.List(member.Id);

            // Display images and find if bookmarked by current user

            Image? parentAvatar = null;
            List<Image> parentImages = [];

            foreach (var i in images)
            {
                //i.Path = $"https://localhost:7025/uploads/{i.Path}";
                i.Path = $"{_configuration["FileRepository"]}/{i.Path}";
                if (i.Type == "avatar")
                {
                    parentAvatar = i;
                }
                else
                {
                parentImages.Add(i);
                }
            }
           
            foreach (var partner in partners)
            {
                var partnerFm = await _familyMemberRepository.Find(partner.WifeId);
                var partnerImages = await _fileRepository.List(partner.WifeId);

                // Display images and find if bookmarked by current user
                Image? partnerAvatar = null;

                List<Image> selectedPartnerImages = [];

                foreach (var i in partnerImages)
                {
                    //i.Path = $"https://localhost:7025/uploads/{i.Path}";
                    i.Path = $"{_configuration["FileRepository"]}/{i.Path}";
                    if (i.Type == "avatar")
                    {
                        partnerAvatar = i;
                    }
                    else
                    {
                        selectedPartnerImages.Add(i);
                    }
                }

                var partnerToDto = ToUserDto(partnerFm!);
                partnerToDto.Images = selectedPartnerImages;
                partnerToDto.Married = partner.Married;
                partnerToDto.Divorced = partner.Divorced;
                partnerToDto.PartnerId = partner.Id;
                partnerToDto.PartnerName = $"{member?.User?.FirstName ?? ""} {member?.User?.MiddleName ?? ""} {member?.User?.LastName ?? ""}";
                partnerToDto.Avatar = partnerAvatar;

                partnerDtos.Add(partnerToDto);
            }

            var node = new TreeOutputDto
            {
                Id = memberId,
                Avatar = parentAvatar,
                FirstName = member?.User?.FirstName ?? "",
                MiddleName = member?.User?.MiddleName ?? "",
                LastName = member?.User?.LastName ?? "",
                FamilyId = member?.FamilyId ?? 0,
                Occupation = member?.Occupation ?? "",
                PlaceOfBirth = member?.PlaceOfBirth ?? "",
                Bio = member?.Bio ?? "",
                Partners = partnerDtos,
                Email = member?.User?.Email ?? "",
                Phone = member?.User?.PhoneNumber ?? "",
                Images = parentImages,
                Gender = member?.Gender ?? "",
                ShowInTree = true,
                Children = new List<TreeOutputDto>(),
                Born = member?.Born ?? "",
                Died = member?.Died,
                FatherId = member?.FatherId,
                MotherId = member?.MotherId,
                CreatedAt = member?.CreatedAt ?? DateTime.Now,
            };


            // Get children of this partnership
            var children = await _familyMemberRepository.GetChildren(memberId, null);

            foreach (var child in children)
            {
                if (visited.Contains(child.Id)) continue;

                var childPartners = await _partnerRepository.Partners(child.Id, PartnerType.Husband);
                if (childPartners.Any())
                {
                    var childNode = await BuildFamilyNode(child.Id, visited);
                    if (childNode != null)
                        node.Children.Add(childNode);
                }
                else
                {
                    var childImages = await _fileRepository.List(child.Id);

                    // Display images and find if bookmarked by current user

                    Image? childAvatar = null;

                    List<Image> selectedChildImages = [];

                    foreach (var i in childImages)
                    {
                        //i.Path = $"https://localhost:7025/uploads/{i.Path}";
                        i.Path = $"{_configuration["FileRepository"]}/{i.Path}";
                        if (i.Type == "avatar")
                        {
                            childAvatar = i;
                        }
                        else
                        {
                            selectedChildImages.Add(i);
                        }
                    }

                    var childNode = new TreeOutputDto
                    {
                        Id = child.Id,
                        Avatar = childAvatar,
                        FirstName = child?.User?.FirstName ?? "",
                        MiddleName = child?.User?.MiddleName ?? "",
                        LastName = child?.User?.LastName ?? "",
                        FamilyId = child?.FamilyId ?? 0,
                        Occupation = child?.Occupation ?? "",
                        PlaceOfBirth = child?.PlaceOfBirth ?? "",
                        Bio = child?.Bio ?? "",
                        ShowInTree = child?.ShowInTree ?? true,
                        Images = selectedChildImages,
                        Email = child?.User?.Email ?? "",
                        Gender = child?.Gender ??   "",
                        Phone = child?.User?.PhoneNumber ?? "",
                        Born = child?.Born ?? "",
                        Died = child?.Died,
                        FatherId = child?.FatherId, 
                        MotherId = child?.MotherId, 
                        CreatedAt = child?.CreatedAt ?? DateTime.Now,
                    };

                    node.Children.Add(childNode);
                    visited.Add(child.Id);
                }
            }

            return node;
        }

        private FamilyMemberOutput ToUserDto(FamilyMember member)
        {
            var user = member.User!;
            return new FamilyMemberOutput
            {
                Id = member.Id,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                ShowInTree = member?.ShowInTree ?? true,
                FamilyId = member?.FamilyId ?? 0,
                Gender = member?.Gender ?? "",
                Born = member?.Born ?? "",
                Died = member?.Died,
                Email = member?.User?.Email ?? "",
                Phone = member?.User?.PhoneNumber ?? "",
                Occupation = member?.Occupation ?? "",
                PlaceOfBirth = member?.PlaceOfBirth ?? "",
                Bio = member?.Bio ??    "",
                FatherId = member?.FatherId,
                MotherId = member?.MotherId, 
                CreatedAt = member?.CreatedAt ?? DateTime.Now,
            };
        }

    }
}
