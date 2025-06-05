namespace familytree_api.Dtos.Auth
{
    public class CheckUserTokenDto
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
