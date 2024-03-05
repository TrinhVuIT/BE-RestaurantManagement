using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Data.RequestModels.User
{
    public class SetPasswordModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and comfirm new password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
