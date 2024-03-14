using RestaurantManagement.Commons;

namespace RestaurantManagement.Data.Entities
{
    //Phiếu nhập kho
    public class StockIn : BaseEntityCommons
    {
        //Số hóa đơn
        public string InvoiceNumber { get; set; }
        //Nhà cung cấp
        public virtual Supplier Supplier { get; set; }
        //Nguyên liệu
        public virtual Ingredient Ingredient { get; set; }
        //Số lượng nhập kho
        public int Quantity { get; set; }
        //Đơn giá của nguyên liệu
        public decimal UnitPrice { get; set; }
        //Tổng số tiền
        public decimal TotalAmount { get; set; }
    }
}
