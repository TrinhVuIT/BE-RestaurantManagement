using Microsoft.AspNetCore.Identity;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities;
using RestaurantManagement.Data.ResponseModels.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using RestaurantManagement.Commons;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace RestaurantManagement.Business.BaseService
{
    public class BaseService : IBaseService
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;
        public BaseService(DataContext context, UserManager<User> userManager, IConfiguration configuration, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        public async Task<TokenModel> GenerateTokenUser(User user, IList<string> roles)
        {
            var secretKeyBytes = Encoding.UTF8.GetBytes(_configuration[Constants.AppSettingKeys.JWT_SECRET]!);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user?.UserName!),
                    new Claim(ClaimTypes.Sid, user?.Id!),
                    user?.Email != null ? new Claim(JwtRegisteredClaimNames.Email, user?.Email!) : null,
                    user?.Email != null ? new Claim(JwtRegisteredClaimNames.Sub, user?.Email!) : null,
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserName", user?.UserName!),
                    new Claim("Id", user?.Id.ToString()!),

                    //roles
                }!),
                Expires = DateTime.Now.AddMinutes(double.Parse(_configuration[Constants.AppSettingKeys.JWT_EXPIREMINUTES]!)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.Aes128CbcHmacSha256)
            };

            foreach (var role in roles)
                tokenDescription?.Subject?.AddClaim(new Claim(ClaimTypes.Role, role));

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            //Lưu database
            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                JwtId = token.Id,
                UserId = user.Id,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.Now,
                ExpiredAt = DateTime.Now.AddMinutes(double.Parse(_configuration[Constants.AppSettingKeys.JWT_EXPIREMINUTES]!))
            };

            await _context.RefreshToken.AddAsync(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<ApiResponse> ReNewToken(TokenModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_configuration[Constants.AppSettingKeys.JWT_SECRET] ?? string.Empty);
            var tokenValidateParam = new TokenValidationParameters
            {
                //tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,

                //ký vào token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = false // ko kiểm tra token hết hạn set false
            };
            try
            {
                //check 1: AccessToken valid format
                var tokenInVerification = jwtTokenHandler.ValidateToken(model.AccessToken, tokenValidateParam, out var validatedToken);

                //check 2: Check alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)//false
                    {
                        return new ApiResponse
                        {
                            Success = false,
                            Message = "Invalid token"
                        };
                    }
                }

                //check 3: Check accessToken expire?
                var expireDateTime = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)!.Value);

                var expireDate = ConvertUnixTimeToDateTime(expireDateTime);
                if (expireDate > DateTime.Now)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "Access token has not yet expired"
                    };
                }

                //check 4: Check refreshtoken exist in DB
                var storedToken = _context.RefreshToken.FirstOrDefault(x => x.Token == model.RefreshToken);
                if (storedToken == null)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token does not exist"
                    };
                }

                //check 5: check refreshToken is used/revoked?
                if (storedToken.IsUsed)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been used"
                    };
                }
                if (storedToken.IsRevoked)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been revoked"
                    };
                }

                //check 6: AccessToken id == JwtId in RefreshToken
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)!.Value;
                if (storedToken.JwtId != jti)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "Token doesn't match"
                    };
                }

                //Update token is used
                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;
                _context.Update(storedToken);
                await _context.SaveChangesAsync();

                //create new token
                var user = await _context.ApplicationUser.SingleOrDefaultAsync(nd => nd.Id == storedToken.UserId);
                var userRoles = await _userManager.GetRolesAsync(user!);
                var token = await GenerateTokenUser(user, userRoles);
                await _signInManager.RefreshSignInAsync(user);
                return new ApiResponse
                {
                    Success = true,
                    Message = "Renew token success",
                    Data = token
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Something went wrong: " + ex.Message
                };
            }
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);

                return Convert.ToBase64String(random);
            }
        }
        private DateTime ConvertUnixTimeToDateTime(long expireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            dateTimeInterval.AddSeconds(expireDate).ToUniversalTime();

            return dateTimeInterval;
        }
    }
}
