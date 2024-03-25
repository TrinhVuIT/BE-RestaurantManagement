using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Business.AuthService;
using RestaurantManagement.Business.BaseService;
using RestaurantManagement.Commons;
using RestaurantManagement.Data.RequestModels.User;
using RestaurantManagement.Data.ResponseModels.User;

namespace RestaurantManagement.Api.Controllers
{
    [Route(Constants.AppSettingKeys.DEFAULT_CONTROLLER_ROUTE)]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IBaseService _baseService;
        public AuthController(IAuthService authService, IBaseService baseService)
        {
            _authService = authService;
            _baseService = baseService;
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var result = await _authService.Register(model);
            if (!result)
                return Problem($"Register invalid to save", $"api/auth/Register", 400);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _authService.Login(model);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            var result = await _authService.SignOutAsync();
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordModel model)
        {
            var userName = User?.Identity?.Name;
            var result = await _authService.ChangePassword(userName!, model);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordModel model)
        {
            var result = await _authService.ForgotPassword(model);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordModel model)
        {
            var result = await _authService.ResetPassword(model);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] TokenModel model)
        {
            var result = await _baseService.ReNewToken(model);

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> VerifyUserTokenAsync(string email, string token, bool isDecodeToken)
        {
            var result = await _authService.VerifyUserToken(email, token, isDecodeToken);

            return Ok(result);
        }
        [HttpPost()]
        public async Task<IActionResult> SetPasswordByAdmin(string email, [FromBody] SetPasswordModel model)
        {
            var result = await _authService.SetPasswordByAdmin(email, model);

            return Ok(result);
        }
    }
}
