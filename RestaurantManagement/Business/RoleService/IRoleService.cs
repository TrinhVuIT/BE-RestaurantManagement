using RestaurantManagement.Data.RequestModels.User;

namespace RestaurantManagement.Business.RoleService
{
    public interface IRoleService
    {
        Task<bool> CreateRole(RoleInputModel model);
        Task<bool> UpdateRole(string id, RoleInputModel model);
        Task<bool> DeleteRole(string id);
        Task<bool> CreateUserRole(UserRoleInputModel model);
    }
}
