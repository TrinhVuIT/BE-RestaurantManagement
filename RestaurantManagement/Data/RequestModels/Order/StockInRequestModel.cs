namespace RestaurantManagement.Data.RequestModels.Order
{
    public class StockInRequestModel
    {
        public string InvoiceNumber { get; set; }
        public long SupplierId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
