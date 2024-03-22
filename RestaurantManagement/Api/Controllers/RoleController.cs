using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Business.RoleService;
using RestaurantManagement.Commons;
using RestaurantManagement.Data.RequestModels.User;

namespace RestaurantManagement.Api.Controllers
{
    [Route(Constants.AppSettingKeys.DEFAULT_CONTROLLER_ROUTE)]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateUserRole([FromBody]UserRoleInputModel model)
        {
            var res = await _roleService.CreateUserRole(model);
            if (!res)
            {
                return Problem($"Create User Role invalid to save", $"api/auth/CreateUserRole", 400);
            }
            return Ok(res);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleInputModel model)
        {
            var res = await _roleService.CreateRole(model);
            if (!res)
            {
                return Problem($"Create Role invalid to save", $"api/auth/CreateRole", 400);
            }
            return Ok(res);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] RoleInputModel model)
        {
            var res = await _roleService.UpdateRole(id, model);
            if (!res)
            {
                return Problem($"Update Role invalid to save", $"api/auth/UpdateRole", 400);
            }
            return Ok(res);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var res = await _roleService.DeleteRole(id);
            if (!res)
            {
                return Problem($"Can't Delete Role", $"api/auth/DeleteRole", 400);
            }
            return Ok(res);
        }
    }
}
