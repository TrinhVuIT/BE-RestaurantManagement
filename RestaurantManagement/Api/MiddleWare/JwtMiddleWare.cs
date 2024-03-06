using Microsoft.IdentityModel.Tokens;
using RestaurantManagement.Commons;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestaurantManagement.Api.MiddleWare
{
    public class JwtMiddleWare : IMiddleware
    {
        private readonly IConfiguration configuration;
        public JwtMiddleWare(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            if (string.IsNullOrEmpty(token))
            {
                token = Convert.ToString(context.Request.Query["access_token"]);
            }

            if (token != null)
            {
                AttachUserToContext(context, token);
            }

            await next(context);
        }

        private void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                byte[] key = Encoding.UTF8.GetBytes(configuration.GetSection(Constants.AppSettingKeys.JWT_SECRET).Value!);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
                var permissions = jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
                context.Items[Constants.AppSettingKeys.PERMISSIONS] = permissions;

                var claims = jwtToken?.Claims.ToList();
                var userIdentity = new ClaimsIdentity(claims, ClaimTypes.Name);
                context.User = new ClaimsPrincipal(userIdentity);

            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
