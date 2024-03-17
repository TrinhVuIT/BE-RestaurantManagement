using RestaurantManagement.Commons;

namespace RestaurantManagement.Data.Entities
{
    //Phiếu xuất kho
    public class StockOut : BaseEntityCommons
    {
        //Lý do xuất kho
        public string Reason { get; set; }
        //Tổng số tiền
        public decimal TotalAmount { get; set; }
    }
}
