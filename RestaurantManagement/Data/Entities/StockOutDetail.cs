using RestaurantManagement.Commons;

namespace RestaurantManagement.Data.Entities
{
    public class StockOutDetail : BaseEntityCommons
    {
        public virtual StockOut StockOut { get; set; }
        //Nguyên liệu
        public virtual Ingredient Ingredient { get; set; }
        //Số lượng xuất kho
        public int Quantity { get; set; }
        //Đơn giá của nguyên liệu
        public decimal UnitPrice { get; set; }
    }
}
