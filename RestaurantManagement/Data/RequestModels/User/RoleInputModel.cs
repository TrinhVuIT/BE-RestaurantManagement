using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Data.RequestModels.User
{
    public class RoleInputModel
    {
        [Required(ErrorMessage = "Phải nhập tên role")]
        [Display(Name = "Tên của Role")]
        [StringLength(100, ErrorMessage = "{0} dài {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Name { set; get; }
    }

    public class UserRoleInputModel
    {
        public string UserName { set; get; }
        public List<RoleInputModel> Roles { set; get; }
    }
}
