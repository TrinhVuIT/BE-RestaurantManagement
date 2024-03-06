using Microsoft.AspNetCore.Identity;
using RestaurantManagement.Business.BaseService;
using RestaurantManagement.Commons;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities;
using RestaurantManagement.Data.RequestModels.User;
using RestaurantManagement.Data.ResponseModels.User;

namespace RestaurantManagement.Business.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;
        private readonly IBaseService _baseService;
        public AuthService(DataContext context, UserManager<User> userManager, IBaseService baseService, IConfiguration configuration, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _baseService = baseService;

        }

        public async Task<bool> Register(RegisterModel model)
        {
            var userExist = await _userManager.FindByNameAsync(model.UserName);
            if (userExist != null)
                throw new Exception(string.Format(Constants.ExceptionMessage.ALREADY_EXIST, nameof(model.UserName)));

            User user = new User()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                NgayTao = DateTime.Now,
                NgayCapNhat = DateTime.Now,
                IsActive = true,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                throw new Exception(string.Format(Constants.ExceptionMessage.FAILED, nameof(model.UserName)));
            return result.Succeeded;
        }
        public async Task<LoginResponseModel> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if(user != null && user.IsActive && !user.IsDeleted && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                //gen token
                var token = await GenerateTokenUser(user, userRoles);

                return new LoginResponseModel
                {
                    Success = true,
                    Message = "Authenticate success",
                    Data = token
                };
            }

            return new LoginResponseModel
            {
                Success = false,
                Message = "Authenticate false",
                User = null,
                Data = null
            };
        }
        public Task<bool> ChangePassword(string userName, ChangePasswordModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ForgotPassword(ForgotPasswordModel model)
        {
            throw new NotImplementedException();
        }



        public Task<bool> ResetPassword(ResetPasswordModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetPassword(string email, SetPasswordModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetPasswordByAdmin(string email, SetPasswordModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> SignOutAsync()
        {
            await _signInManager.SignOutAsync();
            return new ApiResponse
            {
                Success = true,
                Message = "Logout success!"
            };
        }

        public Task<bool> VerifyUserToken(string email, string token, bool isDecodeToken)
        {
            throw new NotImplementedException();
        }
        private async Task<TokenModel> GenerateTokenUser(User user, IList<string> roles)
        {
            return await _baseService.GenerateTokenUser(user, roles);
        }
    }
}
