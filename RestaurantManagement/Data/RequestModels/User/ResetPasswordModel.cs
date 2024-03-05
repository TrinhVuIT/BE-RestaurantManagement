using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Data.RequestModels.User
{
    public class ResetPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("Password", ErrorMessage = "The new password and comfirm new password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
    }
}
