using familytree_api.Database;
using familytree_api.Dtos.Auth;
using familytree_api.Dtos.Emails;
using familytree_api.Dtos.Family;
using familytree_api.Enums;
using familytree_api.Events;
using familytree_api.Models;
using familytree_api.Repositories.FamilyMember;
using familytree_api.Repositories.User;
using familytree_api.Services.Family;
using familytree_api.Services.Token;
using Microsoft.AspNetCore.Identity;

namespace familytree_api.Services.Auth
{
    public class AuthService(
        IUnitOfWork _unitOfWork,
        IUserRepository _userRepository,
        ITokenService _tokenService,
        IEventDispatcher _eventDispatcher,
        IFamilyService _familyService,
        IJwtService _jwtService,
        IHttpContextAccessor _httpContextAccessor,
        IFamilyMemberRepository _familyMemberRepository
        ) : IAuthService
    {
     
        /* Create user with unique email address
         * Create family
         * Add user as family member but keep the show_in_tree flag as false
         * Create a token and send it to the user for account verification
         */

        public async Task SignUp(SignUpDto body)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existingUser = await _userRepository.FindByEmail(body.Email);

                if (existingUser != null)
                {
                    throw new Exception($"A user with this email already exists.");
                }

                // Create an instance of PasswordHasher
                Models.User user = new () { FirstName = body.FirstName, LastName = body.LastName, 
                    Role = UserRoles.Admin.ToString(),
                    Email = body.Email, CreatedAt = DateTime.Now };

                if (body.Password != null)
                {
                    var passwordHasher = new PasswordHasher<Models.User>();

                    user.Password = passwordHasher.HashPassword(user, body.Password);
                }

                var result = await _userRepository.Create(user);

                // Create a family
                FamilyInputDto familyBody = new()
                {
                    Name=body.Family,
                    Origin="",
                    CreatedAt = DateTime.Now,
                };

                var family = await _familyService.CreateFamily(familyBody);

                // Add the user as a family member
                FamilyMember familyMember = new()
                {
                    UserId = user.Id,
                    FamilyId = family.Id,
                    Born = "",
                    ShowInTree = false,
                    CreatedAt = DateTime.Now
                };
                
                await _familyMemberRepository.Create(familyMember);

                // Record validation token and send user validation code

                var verificationToken = await _tokenService.Create(user.Id);

                EmailMessage emailBody = new() { Subject = "Welcome To Family Tree", To = body.Email, ValidationToken = verificationToken.Code, Name = $"{user.FirstName} {user.LastName}" };
                
                await _eventDispatcher.DispatchAsync(emailBody);
                
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();

                throw;
            }
        }


        /*
         * Sign in to system
         * 
         */
        public async Task<string> SignIn(SignInDto body)
        {
            try
            {
                // Find the user in the database by their username or email
                //var user = await _userRepository.FindByEmail(body.Email);

                var user =  await _userRepository.FindByEmail(body.Email) ?? throw new Exception("Invalid credentials.");
                var passwordHasher = new PasswordHasher<Models.User>();

                // Verify the entered password against the stored hashed password
                var verificationResult = passwordHasher.VerifyHashedPassword(user, user.Password!, body.Password);

                // If the password is incorrect, return unauthorized
                if (verificationResult == PasswordVerificationResult.Failed)
                {
                    throw new Exception("Invalid credentials");

                }

                // Check if email address is verified
                if (user.EmailVerified == false) throw new Exception("Please verifiy your email address before accessing platform");

                return _jwtService.GenerateToken(user);
            }
            catch
            {
                throw;
            }

        }

        /*
         * Change old password
         * 
         */
        public async Task ChangePassword(ChangePasswordDto body)
        {
            try
            {
                var userProfile = _httpContextAccessor.HttpContext?.Items["UserProfile"] ?? throw new Exception("Unauthorized access");

                int UserId = 0;

                if (userProfile is Models.User userModel) UserId = userModel.Id;

                var user = await _userRepository.FindById(UserId) ?? throw new Exception("User not found!");
               
                var passwordHasher = new PasswordHasher<Models.User>();

                // Verify the entered password against the stored hashed password
                var verificationResult = passwordHasher.VerifyHashedPassword(user, user.Password!, body.OldPassword);

                // If the password is incorrect, return unauthorized
                if (verificationResult == PasswordVerificationResult.Failed)
                {
                    throw new Exception("Invalid password.");
                }

                user.Password = passwordHasher.HashPassword(user, body.NewPassword);

                await _userRepository.Update(user);

            }
            catch
            {
                throw;
            }
        }

        /*
         * Request for a password reset and receive an email with a token for that
         * 
         */
        public async Task ResetPasswordRequest(string Email)
        {
            try
            {
                var user = await _userRepository.FindByEmail(Email) ?? throw new Exception("No user found with this email address");

                var verificationToken = await _tokenService.Create(user.Id);

                PasswordReset emailBody = new() { Subject = "Password Reset for Family Tree Account", To = Email, 
                    ValidationToken = verificationToken.Code, Name = $"{user.FirstName} {user.LastName}" };

                await _eventDispatcher.DispatchAsync(emailBody);
            }
            catch
            {
                throw;
            }
        }

        /*
         * Check if reset token/code and email combination is valid 
         */
        public async Task<bool> PasswordResetTokenExists(VerifyTokenDto body)
        {
            try
            {
                var user = await _userRepository.FindByEmail(body.Email) ?? throw new Exception("No user found with this email address");

                var token = await _tokenService.UserAndTokenExist(new CheckUserTokenDto() { Code = body.Code, UserId = user.Id });

                return token != null;
            }
            catch
            {
                throw;
            }
        }

        /*
         * After veriying reset token/code and email combination, reset password and login
         * 
         */
        public async Task<string> ResetPassword(ResetPasswordDto body)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var user = await _userRepository.FindByEmail(body.Email) ?? throw new Exception("No user found with this email address");

                var token = await _tokenService.UserAndTokenExist(new CheckUserTokenDto() { Code = body.Code, UserId = user.Id });

                var passwordHasher = new PasswordHasher<Models.User>();

                user.Password = passwordHasher.HashPassword(user, body.Password);

                await _userRepository.Update(user);

                _tokenService.Delete(user.Id);

                await _unitOfWork.CommitAsync();

                return _jwtService.GenerateToken(user);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        /*
         * Validate a user account from token/code and email combination
         * 
         */

        public async Task<string> ValidateUserAccount(VerifyTokenDto body)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                //var user = await _userRepository.FindByEmail(body.Email) ?? throw new Exception("User does not exist!");

                var user = await _userRepository.FindByEmail(body.Email) ?? throw new Exception("Invalid email or password.");

                var token = await _tokenService.UserAndTokenExist(new CheckUserTokenDto() { Code = body.Code, UserId = user.Id })
                    ??
                    throw new Exception("Invalid account verification credentials!");

                // Change email verified = true
                user.EmailVerified = true;

                await _userRepository.Update(user);

                // Delete the token
                _tokenService.Delete(token.Id);

                await _unitOfWork.CommitAsync();


                return _jwtService.GenerateToken(user);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();

                throw;
            }

        }
    }
}
