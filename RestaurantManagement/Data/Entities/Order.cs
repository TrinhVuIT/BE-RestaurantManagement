using RestaurantManagement.Commons;
using static RestaurantManagement.Commons.Enums;

namespace RestaurantManagement.Data.Entities
{
    //Đơn đặt hàng
    public class Order : BaseEntityCommons
    {
        //Nhà cung cấp
        public virtual Supplier Supplier { get; set; }
        //Ngày hẹn giao hàng
        public DateTime? DeliveryAppointment {  get; set; }
        public StatusOrder StatusOrder { get; set; }
    }
}
