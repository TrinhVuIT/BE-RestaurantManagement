using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Business.AuthService;
using RestaurantManagement.Business.BaseService;
using RestaurantManagement.Commons;
using RestaurantManagement.Data.RequestModels.User;

namespace RestaurantManagement.Api
{
    [Route(Constants.AppSettingKeys.DEFAULT_CONNECTION)]
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
            if(!result)
                return Problem($"Register invalid to save", $"api/auth/Register", 400);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _authService.Login(model);

            return Ok(result);
        }
    }
}
