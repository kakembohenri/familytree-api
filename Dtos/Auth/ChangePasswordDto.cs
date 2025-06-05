using System.ComponentModel.DataAnnotations;

namespace familytree_api.Dtos.Auth
{
    public class ChangePasswordDto
    {
        public string OldPassword { get; set; } = string.Empty;

        [MinLength(6, ErrorMessage = "New Password must be at least 6 characters.")]
        public string NewPassword { get; set; } = string.Empty;

        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
