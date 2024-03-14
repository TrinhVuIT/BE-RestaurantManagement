using Microsoft.AspNetCore.Identity;
using RestaurantManagement.Data.Entities.Address;
using static RestaurantManagement.Commons.Enums;

namespace RestaurantManagement.Data.Entities
{
    public class User : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Code { get; set; }
        public Sex? Sex { get; set; }
        public DateTime? Birthday { get; set; }
        //Quốc gia
        public virtual ListOfCountries? Country { get; set; }
        //Tỉnh thành
        public virtual Provinces? Province { get; set; }
        //Quận huyện
        public virtual Districts? District { get; set; }
        //Xã phường
        public virtual Wards? Wards { get; set; }
        public string? Address { get; set; }
        //Chức vụ
        public virtual Position? Position { get; set; }
        public string? Avatar { get; set; }
        public bool IsDeleted { get; set; }
        public string? NguoiTao { get; set; }
        public DateTime? NgayTao { get; set; }
        public string? NguoiCapNhat { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
