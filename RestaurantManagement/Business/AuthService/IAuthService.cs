using RestaurantManagement.Data.RequestModels.User;
using RestaurantManagement.Data.ResponseModels.User;

namespace RestaurantManagement.Business.AuthService
{
    public interface IAuthService
    {
        Task<bool> Register(RegisterModel model);
        Task<LoginResponseModel> Login(LoginModel model);
        Task<ApiResponse> SignOutAsync();
        Task<bool> ChangePassword(string userName, ChangePasswordModel model);
        Task<bool> ForgotPassword(ForgotPasswordModel model);
        Task<bool> ResetPassword(ResetPasswordModel model);
        Task<bool> SetPassword(string email, SetPasswordModel model);
        Task<bool> VerifyUserToken(string email, string token, bool isDecodeToken);
        Task<bool> SetPasswordByAdmin(string email, SetPasswordModel model);
    }
}
