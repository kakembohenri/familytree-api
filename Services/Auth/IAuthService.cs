using familytree_api.Dtos.Auth;
using familytree_api.Dtos.Emails;

namespace familytree_api.Services.Auth
{
    public interface IAuthService
    {
        Task SignUp(SignUpDto body);
        Task<string> SignIn(SignInDto body);
        Task<string> ValidateUserAccount(VerifyTokenDto body);
        Task<string> ResetPassword(ResetPasswordDto body);
        Task ChangePassword(ChangePasswordDto body);
        Task<bool> PasswordResetTokenExists(VerifyTokenDto body);
        Task ResetPasswordRequest(string Email);
    }
}
