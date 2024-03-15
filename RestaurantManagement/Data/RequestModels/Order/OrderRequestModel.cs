namespace RestaurantManagement.Data.RequestModels.Order
{
    public class OrderRequestModel
    {
        public long SupplierId { get; set; }
        public DateTime? DeliveryAppointment { get; set; }
    }
}
