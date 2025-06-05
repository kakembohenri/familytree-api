using familytree_api.Services.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace familytree_api.Middleware
{
    public class JwtMiddleware (
         RequestDelegate _next,
        IServiceScopeFactory _serviceScopeFactory
        )
    {

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();

                    {
                        var userProfileClaim = jwtService.ExtractUserIdFromToken(token);

                        if (userProfileClaim != null)
                        {
                            httpContext.Items["UserProfile"] = userProfileClaim;
                        }
                    }
                }

            await _next(httpContext); // Proceed to the next middleware
        }

    }
}
