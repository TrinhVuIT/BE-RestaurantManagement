using RestaurantManagement.Data.Entities.Address;
using RestaurantManagement.Data.Entities;
using static RestaurantManagement.Commons.Enums;

namespace RestaurantManagement.Data.ResponseModels.User
{
    public class UserResponse
    {
        public string? Id { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set;}
        public string? PhoneNumber { get; set; }
        public string? Code { get; set; }
        public Sex? Sex { get; set; }
        public DateTime? Birthday { get; set; }
        public virtual ListOfCountries? Country { get; set; }
        public virtual Provinces? Province { get; set; }
        public virtual Districts? District { get; set; }
        public virtual Wards? Wards { get; set; }
        public string? Address { get; set; }
        public virtual Position? Position { get; set; }
        public string? Avatar { get; set; }
        public bool IsActive { get; set; }
        public DateTime? NgayTao { get; set; }
    }
}
