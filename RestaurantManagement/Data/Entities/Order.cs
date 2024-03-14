using RestaurantManagement.Commons;
using static RestaurantManagement.Commons.Enums;

namespace RestaurantManagement.Data.Entities
{
    //Đơn đặt hàng
    public class Order : BaseEntityCommons
    {
        //Nhà cung cấp
        public virtual Supplier Supplier { get; set; }
        //Nguyên liệu
        public virtual Ingredient Ingredient { get; set; }
        //Số lượng đặt hàng
        public int Quantity { get; set; }
        //Ngày hẹn giao hàng
        public DateTime? DeliveryAppointment {  get; set; }
        public StatusOrder StatusOrder { get; set; }
    }
}
