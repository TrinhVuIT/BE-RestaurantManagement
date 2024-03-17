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
        //Tổng số tiền
        public decimal TotalAmount { get; set; }
    }
}
