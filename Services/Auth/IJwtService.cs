namespace familytree_api.Services.Auth
{
   
    public interface IJwtService
    {
        string GenerateToken(Models.User user);
        Models.User? ExtractUserIdFromToken(string token);
    }
}
