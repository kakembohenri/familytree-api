using familytree_api.Dtos.Auth;

namespace familytree_api.Services.Token
{
    public interface ITokenService
    {
        Task<Models.Token> Create(int userId);
        Task<Models.Token?> UserAndTokenExist(CheckUserTokenDto body);
        void Delete(int id);
    }
}
