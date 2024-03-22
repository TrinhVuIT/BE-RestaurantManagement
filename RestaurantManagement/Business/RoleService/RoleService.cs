using Microsoft.AspNetCore.Identity;
using RestaurantManagement.Commons;
using RestaurantManagement.Data;
using RestaurantManagement.Data.RequestModels.User;

namespace RestaurantManagement.Business.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly DataContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public RoleService(DataContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<bool> CreateRole(RoleInputModel model)
        {
            var roleExits = await _roleManager.RoleExistsAsync(model.Name);
            if (roleExits)
                throw new Exception(string.Format(Constants.ExceptionMessage.ALREADY_EXIST, nameof(model.Name)));

            IdentityRole role = new IdentityRole { Name = model.Name };
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                throw new Exception(string.Format(Constants.ExceptionMessage.FAILED, nameof(model.Name)));

            return result.Succeeded;
        }

        public async Task<bool> DeleteRole(string id)
        {
            var roleExist = await _roleManager.FindByIdAsync(id);
            if (roleExist == null)
                throw new Exception(string.Format(Constants.ExceptionMessage.NOT_FOUND, nameof(id)));

            var result = await _roleManager.DeleteAsync(roleExist);

            return result.Succeeded;
        }

        public async Task<bool> UpdateRole(string id, RoleInputModel model)
        {
            var roleExist = await _roleManager.FindByIdAsync(id);
            if (roleExist == null)
                throw new Exception(string.Format(Constants.ExceptionMessage.NOT_FOUND, nameof(id)));

            var roleNameExits = await _roleManager.RoleExistsAsync(model.Name);
            if (roleNameExits)
                throw new Exception(string.Format(Constants.ExceptionMessage.ALREADY_EXIST, nameof(model.Name)));

            roleExist.Name = model.Name;

            var result = await _roleManager.UpdateAsync(roleExist);
            if (!result.Succeeded)
                throw new Exception(string.Format(Constants.ExceptionMessage.FAILED, nameof(model.Name)));

            return result.Succeeded;
        }

        public async Task<bool> CreateUserRole(UserRoleInputModel model)
        {
            bool result = false;
            var userExist = await _userManager.FindByNameAsync(model.UserName);
            if (userExist == null)
                throw new Exception(string.Format(Constants.ExceptionMessage.NOT_FOUND, nameof(model.UserName)));

            if (model.Roles.Any())
            {
                foreach (var role in model.Roles)
                {
                    var roleExist = await _roleManager.FindByNameAsync(role.Name);
                    if (roleExist != null && !string.IsNullOrEmpty(roleExist.Name))
                    {
                        result = true;
                        await _userManager.AddToRoleAsync(userExist, roleExist.Name);
                    }
                }
            }
            return result;
        }
    }
}
