using familytree_api.Enums;

namespace familytree_api.Dtos.Emails
{
    public class EmailMessage
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? ValidationToken { get; set; }
        public EmailActions Action { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Inviter { get; set; } = string.Empty;
        public string InviterEmail { get; set; } = string.Empty;
    }
}
