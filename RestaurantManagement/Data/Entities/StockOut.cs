using RestaurantManagement.Commons;

namespace RestaurantManagement.Data.Entities
{
    //Phiếu xuất kho
    public class StockOut : BaseEntityCommons
    {
        //Lý do xuất kho
        public string Reason { get; set; }
        public virtual Ingredient Ingredient { get; set; }
        //Số lượng xuất kho
        public int Quantity { get; set; }
        //Đơn giá của nguyên liệu
        public decimal UnitPrice { get; set; }
        //Tổng số tiền
        public decimal TotalAmount { get; set; }
    }
}
