namespace familytree_api.Dtos.AppSettings
{
    public class JWTConfig
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set;} = string.Empty;
    }
}
