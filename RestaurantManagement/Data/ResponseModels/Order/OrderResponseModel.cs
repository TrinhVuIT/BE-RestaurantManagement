using static RestaurantManagement.Commons.Enums;

namespace RestaurantManagement.Data.ResponseModels.Order
{
    public class OrderResponseModel
    {
        public SupplierMapper Supplier { get; set; }
        public DateTime? DeliveryAppointment { get; set; }
        public StatusOrder StatusOrder { get; set; }
    }
    public class SupplierMapper
    {
        public long Id { get; set; }
        public string SupplierName { get; set; }
        public string? SupplierAddress { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
