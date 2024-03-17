using RestaurantManagement.Commons;

namespace RestaurantManagement.Data.Entities
{
    public class StockInDetail : BaseEntityCommons
    {
        public virtual StockIn StockIn { get; set; }
        //Nguyên liệu
        public virtual Ingredient Ingredient { get; set; }
        //Số lượng nhập kho
        public int Quantity { get; set; }
        //Đơn giá của nguyên liệu
        public decimal UnitPrice { get; set; }
    }
}
