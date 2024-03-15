using static RestaurantManagement.Commons.Enums;

namespace RestaurantManagement.Data.ResponseModels.Order
{
    public class OrderResponseModel
    {
        public long Id { get; set; }
        public SupplierMapper Supplier { get; set; }
        public DateTime? DeliveryAppointment { get; set; }
        public StatusOrder StatusOrder { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public string? NguoiTao { get; set; }
        public string? NguoiCapNhat { get; set; }
    }
    public class SupplierMapper
    {
        public long Id { get; set; }
        public string SupplierName { get; set; }
        public string? SupplierAddress { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
