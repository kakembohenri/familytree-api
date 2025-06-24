using System.ComponentModel.DataAnnotations;

namespace familytree_api.Dtos.Auth
{
    public class ResetPasswordDto
    {
        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int UserId { get; set; }

        [MinLength(6, ErrorMessage = "Password must be atleast 6 characters")]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string PasswordConfirmation { get; set; } = string.Empty;
    }
}
