namespace familytree_api.Dtos.Auth
{
    public class VerifyTokenDto
    {
        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
