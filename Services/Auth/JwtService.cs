using familytree_api.Dtos.AppSettings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace familytree_api.Services.Auth
{
    public class JwtService(
        IOptions<JWTConfig> jwtConfig
        ) : IJwtService
    {
        private readonly JWTConfig _jwtConfig = jwtConfig.Value;

        public string GenerateToken(Models.User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //var userProfile = _mapper.Map<UserOutputDtoSmallCase>(user);
            user.Password = "";

            var userProfile = JsonSerializer.Serialize<Models.User>(user);

            var claims = new[]
            {
            //new Claim("userProfile", JsonSerializer.Serialize<UserOutputDtoSmallCase>(userProfile)),
            new Claim("userProfile", userProfile),
            //new Claim(ClaimTypes.Role, user?.UserType?.Name!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Models.User? ExtractUserIdFromToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // Extract the "userProfile" claim
                var userProfileClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "userProfile")?.Value;
                if (userProfileClaim == null) return null;

                // Deserialize the user profile to access UserId
                var deserilizedUser = JsonSerializer.Deserialize<Models.User>(userProfileClaim);
                //var deserilizedUser = JsonSerializer.Deserialize<UserOutputDtoSmallCase>(userProfileClaim);

                //var user = _mapper.Map<Models.User>(deserilizedUser);

                return deserilizedUser;
            }
            catch (Exception)
            {
                // Handle any errors (token invalid, claim not found, etc.)
                return null;
            }
        }

    }
}
