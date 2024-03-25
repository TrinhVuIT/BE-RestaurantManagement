using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Business.BaseService;
using RestaurantManagement.Business.EmailCofigServices;
using RestaurantManagement.Commons;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities;
using RestaurantManagement.Data.RequestModels.User;
using RestaurantManagement.Data.ResponseModels.User;
using System.Data;
using System.Text;

namespace RestaurantManagement.Business.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IBaseService _baseService;
        private readonly IEmailConfigServices _emailConfigServices;
        public AuthService(DataContext context, UserManager<User> userManager, IEmailConfigServices emailConfigServices, IBaseService baseService, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _baseService = baseService;
            _emailConfigServices = emailConfigServices;

        }

        public async Task<bool> Register(RegisterModel model)
        {
            var userExist = await _userManager.FindByNameAsync(model.UserName);
            if (userExist != null)
                throw new Exception(string.Format(Constants.ExceptionMessage.ALREADY_EXIST, nameof(model.UserName)));

            long codeUser = GetCodeUserLatest().Result != null ? long.Parse(GetCodeUserLatest().Result!) : 0;
            if (codeUser > 0)
            {
                var codeUserExist = await _context.ApplicationUser
                    .AnyAsync(x => !x.IsDeleted && x.IsActive && x.Code == codeUser.ToString());

                if (codeUserExist)
                    codeUser++;
            }

            User user = new User()
            {
                Email = model.Email,
                Code = codeUser.ToString("D7"),
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                NgayTao = DateTime.Now,
                NgayCapNhat = DateTime.Now,
                NguoiTao = Constants.JobScheduleOptions.NameSystemJob,
                NguoiCapNhat = Constants.JobScheduleOptions.NameSystemJob
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
                var token = await _baseService.GenerateTokenUser(user, userRoles);

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
        public async Task<ApiResponse> SignOutAsync()
        {
            await _signInManager.SignOutAsync();
            return new ApiResponse
            {
                Success = true,
                Message = "Logout success!"
            };
        }
        public async Task<bool> ChangePassword(string userName, ChangePasswordModel model)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if(user == null)
                throw new Exception(string.Format(Constants.ExceptionMessage.NOT_FOUND, nameof(userName)));

            var changePassword = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if(!changePassword.Succeeded)
                throw new Exception(string.Format(Constants.ExceptionMessage.FAILED, nameof(userName)));

            return changePassword.Succeeded;
        }


        public async Task<bool> ForgotPassword(ForgotPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user == null)
                throw new Exception(string.Format(Constants.ExceptionMessage.NOT_FOUND, nameof(model.Email)));

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var fullName = user?.FullName ?? user?.UserName;
            var result = await _emailConfigServices.SendEmailForgotPasswordAsync(fullName!, model.Email, code);
            if(!result)
                throw new Exception(string.Format(Constants.ExceptionMessage.FAILED, nameof(model.Email)));

            return result;
        }



        public async Task<bool> ResetPassword(ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new Exception(string.Format(Constants.ExceptionMessage.NOT_FOUND, nameof(model.Email)));
            }
            if (model?.Token == null)
            {
                throw new Exception(string.Format(Constants.ExceptionMessage.FAILED, "Token"));
            }
            model.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Format(Constants.ExceptionMessage.FAILED, "Token"));
            }

            return result.Succeeded;
        }

        public async Task<bool> SetPassword(string email, SetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception(string.Format(Constants.ExceptionMessage.FAILED, nameof(email)));
            }
            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Format(Constants.ExceptionMessage.FAILED, nameof(email)));
                }

                return result.Succeeded;
            }
            return false;
        }

        public async Task<bool> SetPasswordByAdmin(string email, SetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception(string.Format(Constants.ExceptionMessage.FAILED, nameof(email)));
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (string.IsNullOrEmpty(token))
            {
                throw new Exception(string.Format(Constants.ExceptionMessage.FAILED, nameof(token)));
            }
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
            if (!result.Succeeded)
            {
                throw new Exception(string.Format(Constants.ExceptionMessage.FAILED, nameof(email)));
            }

            return result.Succeeded;
        }

        public async Task<bool> VerifyUserToken(string email, string token, bool isDecodeToken)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception(string.Format(Constants.ExceptionMessage.NOT_FOUND, nameof(email)));
            }
            if (token == null)
            {
                throw new Exception(string.Format(Constants.ExceptionMessage.FAILED, "token"));
            }
            if (isDecodeToken)
            {
                token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            }

            var purpose = UserManager<User>.ResetPasswordTokenPurpose;
            if (!await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, purpose, token))
            {
                throw new Exception(string.Format(Constants.ExceptionMessage.FAILED, "token"));
            }

            return true;
        }

        private async Task<string?> GetCodeUserLatest()
        {
            var result = await _userManager.Users.OrderByDescending(u => u.NgayTao).FirstOrDefaultAsync();
            return result?.Code;
        }
    }
}
