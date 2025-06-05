using System.ComponentModel.DataAnnotations;

namespace familytree_api.Dtos.Auth
{
    public class SignInDto
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

    }
}
