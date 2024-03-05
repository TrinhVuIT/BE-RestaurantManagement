using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Data.RequestModels.User
{
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        //Mật khẩu hiện tại
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        //Mật khẩu mới
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and the re-entered password do not match.")]
        //Nhập lại mật khẩu mới
        public string ConfirmPassword { get; set; }
    }
}
