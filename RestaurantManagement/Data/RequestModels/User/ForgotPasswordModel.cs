using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Data.RequestModels.User
{
    public class ForgotPasswordModel
    {
        [Display(Name = "Login Name")]
        //Tên Đăng Nhập
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
