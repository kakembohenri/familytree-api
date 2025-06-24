using familytree_api.Database;
using familytree_api.Dtos.Emails;
using familytree_api.Dtos.User;
using familytree_api.Enums;
using familytree_api.Events;
using familytree_api.Repositories.FamilyMember;
using familytree_api.Repositories.User;
using Microsoft.AspNetCore.Identity;

namespace familytree_api.Services.User
{
    public class UserService(
        IUserRepository _userRepository,
        IFamilyMemberRepository _familyMemberRepository,
        IHttpContextAccessor _httpContextAccessor,
        IEventDispatcher _eventDispatcher,
        IUnitOfWork _unitWork

        ) : IUserService
    {
     
        /*
         * Create a user with family member
         * Send email to user after creating them with password
         * 
         */
        public async Task Create(UserInputDto body)
        {
            await _unitWork.BeginTransactionAsync();
            try
            {
                var userProfile = _httpContextAccessor.HttpContext?.Items["UserProfile"] ?? throw new Exception("Unauthorized access");

                int loggedInUserId = 0;

                if (userProfile is Models.User userModel)
                {
                    loggedInUserId = userModel.Id;

                    if (userModel.Role != UserRoles.Admin.ToString()) throw new Exception("User doesnt have permissions to perform this action!");
                }

                var loggedInUser = await _familyMemberRepository.FindByUserId(loggedInUserId) ?? throw new Exception("User doesnt exist!");

                // Check if email is unique
                var emailExists = await _userRepository.FindByEmail(body.Email);

                if (emailExists != null) throw new Exception("Email Address already in use!");

                // Create user instance for family member
                Models.User user = new()
                {
                    FirstName = body.FirstName,
                    LastName = body.LastName,
                    Role = UserRoles.Viewer.ToString(),
                    Email = body.Email,
                    PhoneNumber = body.Phone,
                    EmailVerified = true,
                    CreatedAt = body.CreatedAt
                };

                var passwordHasher = new PasswordHasher<Models.User>();

                string password = GenerateRandomString();

                user.Password = passwordHasher.HashPassword(user, password);

                await _userRepository.Create(user);

                // Create family member instance

                Models.FamilyMember familyMember = new()
                {
                    UserId = user.Id,
                    FamilyId = loggedInUser.FamilyId,
                    Gender = body.Gender,
                    ShowInTree = false,
                    CreatedAt = body.CreatedAt
                };

                await _familyMemberRepository.Create(familyMember);

                UserInvite emailBody = new() { Subject = "Welcome To Family Tree", To = body.Email,  Name = $"{user.FirstName} {user.LastName}", Password= password,
                Inviter= $"{loggedInUser?.User?.FirstName} {loggedInUser?.User?.MiddleName} {loggedInUser?.User?.LastName}", InviterEmail=loggedInUser?.User?.Email??""
                };

                await _eventDispatcher.DispatchAsync(emailBody);

                await _unitWork.CommitAsync();

            }
            catch
            {
                await _unitWork.RollbackAsync();

                throw;
            }
        }

        /*
         * Get paginated users
         * 
         */
        public async Task<UserFilterOutputDto<UserOutputDto>> List(UserFilterDto filter)
        {
            try
            {
                var userProfile = _httpContextAccessor.HttpContext?.Items["UserProfile"] ?? throw new Exception("Unauthorized access");

                int loggedInUserId = 0;

                if (userProfile is Models.User userModel)
                {
                    loggedInUserId = userModel.Id;

                    if (userModel.Role != UserRoles.Admin.ToString()) throw new Exception("User doesnt have permissions to perform this action!");
                }

                var loggedInUser = await _familyMemberRepository.FindByUserId(loggedInUserId) ?? throw new Exception("User doesnt exist!");

                filter.FamilyId = loggedInUser.FamilyId;

                return await _userRepository.List(filter);
            }
            catch
            {
                throw;
            }
        }

        /*
         * Update user details including password if IsPasswordChanged is true
         * If password or email is changed, send the user an email with their new credentials
         * 
         */
        public async Task Update(UserUpdateDto body)
        {
            await _unitWork.BeginTransactionAsync();
            try
            {
                var userProfile = _httpContextAccessor.HttpContext?.Items["UserProfile"] ?? throw new Exception("Unauthorized access");

                int loggedInUserId = 0;

                if (userProfile is Models.User userModel)
                {
                    loggedInUserId = userModel.Id;

                    if (userModel.Role != UserRoles.Admin.ToString()) throw new Exception("User doesnt have permissions to perform this action!");
                }

                var loggedInUser = await _familyMemberRepository.FindByUserId(loggedInUserId) ?? throw new Exception("User doesnt exist!");

                var userExists = await _userRepository.FindById(body.Id) ?? throw new Exception("User doesn't exist!");

                // Check if email has changed

                if(userExists.Email != body.Email)
                {

                    // Check if email is unique
                    var emailExists = await _userRepository.FindByEmail(body.Email);

                     if (emailExists != null) throw new Exception("Email Address already in use!");

                     userExists.Email = body.Email;
                }

                userExists.PhoneNumber = body.Phone;
                userExists.FirstName = body.FirstName;
                userExists.LastName = body.LastName;
                userExists.MiddleName = body.MiddleName ?? "";
                userExists.Role = body.Role;

                // Check if IsPasswordChanged = true

                if (body.IsPasswordChanged)
                {
                    var passwordHasher = new PasswordHasher<Models.User>();

                    userExists.Password = passwordHasher.HashPassword(userExists, body.Password);

                    CredentialsChange emailBody = new()
                    {
                        Subject = "Family tree credentials update",
                        To = body.Email,
                        Name = $"{userExists.FirstName} {userExists.LastName}",
                        Password = body.Password,
                        Inviter = $"{loggedInUser?.User?.FirstName} {loggedInUser?.User?.MiddleName} {loggedInUser?.User?.LastName}"
                    };

                    await _eventDispatcher.DispatchAsync(emailBody);
                }

                await _userRepository.Update(userExists);

                await _unitWork.CommitAsync();
            }
            catch
            {
                await _unitWork.RollbackAsync();
                throw;
            }
        }

        public static string GenerateRandomString(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
