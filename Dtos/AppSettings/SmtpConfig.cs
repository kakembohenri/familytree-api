namespace familytree_api.Dtos.AppSettings
{
    public class SmtpConfig
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string From { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
        public bool EnableSsl { get; set; }
    }
}
