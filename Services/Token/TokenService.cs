using familytree_api.Dtos.Auth;
using familytree_api.Repositories.Token;

namespace familytree_api.Services.Token
{
    public class TokenService(
        ITokenRepository _tokenRepository
        ): ITokenService
    {
        public async Task<Models.Token> Create(int userId)
        {
            string validationToken = Guid.NewGuid().ToString("N");

            Models.Token token = new()
            {
                CreatedAt = DateTime.Now,
                Code = validationToken,
                UserId = userId,
            };

            return await _tokenRepository.Create(token);
        }

        public async Task<Models.Token?> UserAndTokenExist(CheckUserTokenDto body)
        {
            return await _tokenRepository.UserTokenCheck(body);
        }

        public void Delete(int id)
        {
            _tokenRepository.Delete(id);
        }
    }
}
