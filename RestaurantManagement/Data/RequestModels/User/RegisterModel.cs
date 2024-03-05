using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Data.RequestModels.User
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
