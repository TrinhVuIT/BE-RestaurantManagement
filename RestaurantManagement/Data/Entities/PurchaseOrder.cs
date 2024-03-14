using RestaurantManagement.Commons;
using static RestaurantManagement.Commons.Enums;

namespace RestaurantManagement.Data.Entities
{
    //Phiếu mua hàng
    public class PurchaseOrder : BaseEntityCommons
    {
        //Người mua hàng
        public virtual User Customer { get; set; }
        //Món ăn
        public virtual Food Food { get; set; }
        //Số lượng món ăn
        public int Quantity { get; set; }
        //Tổng giá tiền
        public decimal TotalPrice { get; set; }
        public StatusOrder Status { get; set; }

    }
}
