using familytree_api.Dtos.Auth;

namespace familytree_api.Repositories.Token
{
    public interface ITokenRepository
    {
        Task<Models.Token> Create(Models.Token body);
        Task<Models.Token?> UserTokenCheck(CheckUserTokenDto input);
        public void Delete(int id);
    }
}
