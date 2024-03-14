namespace RestaurantManagement.Commons
{
    public class Enums
    {
        public enum Sex
        {
            Male = 1, // Nam
            Female
        }
        public enum StatusOrder
        {
            Pending = 1, // Đơn hàng đang chờ xử lý
            Processing, //  Đơn hàng đang được xử lý
            Completed, // Đơn hàng đã hoàn thành
            Cancelled, // Đơn hàng đã bị hủy
            Shipped, //  Đơn hàng đã được vận chuyển
            Delivered, // Đơn hàng đã được giao hàng
            Returned, // Đơn hàng đã được trả lại
            Refunde // Đơn hàng đã được hoàn tiền
        }
    }
}
