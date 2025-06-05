using familytree_api.Database;
using familytree_api.Dtos.Auth;
using Microsoft.EntityFrameworkCore;

namespace familytree_api.Repositories.Token
{
    public class TokenRepository(
        AppDbContext _context
        ):ITokenRepository
    {
        public async Task<Models.Token> Create(Models.Token body)
        {
            await _context.Token.AddAsync(body);
            await _context.SaveChangesAsync();
            return body;
        }

        public async Task<Models.Token?> UserTokenCheck(CheckUserTokenDto input)
        {
            return await _context.Token.Where(t => t.UserId == input.UserId && t.Code == input.Code).SingleOrDefaultAsync();
            //return new Models.VerificationToken();
        }

        public void Delete(int id)
        {
            var token = _context.Token.Find(id);
            if (token != null)
            {
                _context.Token.Remove(token);
                _context.SaveChanges();
            }
        }
    }
}
