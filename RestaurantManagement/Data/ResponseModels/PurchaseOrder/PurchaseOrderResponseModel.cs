using RestaurantManagement.Data.Entities.Address;
using static RestaurantManagement.Commons.Enums;

namespace RestaurantManagement.Data.ResponseModels.PurchaseOrder
{
    public class PurchaseOrderResponseModel
    {
        public long Id { get; set; }
        public UserMapper? Customer { get; set; }
        public string? CustomerOther { get; set; }
        public string? AddressCustomerOther { get; set; }
        public decimal TotalPrice { get; set; }
        public StatusOrder Status { get; set; }
        public string? NguoiTao { get; set; }
        public DateTime? NgayTao { get; set; }
        public string? NguoiCapNhat { get; set; }
        public DateTime? NgayCapNhat { get; set; }
    }
    public class UserMapper
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Avatar { get; set; }
        //Quốc gia
        public virtual ListOfCountries? Country { get; set; }
        //Tỉnh thành
        public virtual Provinces? Province { get; set; }
        //Quận huyện
        public virtual Districts? District { get; set; }
        //Xã phường
        public virtual Wards? Wards { get; set; }
        public string? Address { get; set; }
    }
}
