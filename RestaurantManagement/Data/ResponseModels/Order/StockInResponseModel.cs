namespace RestaurantManagement.Data.ResponseModels.Order
{
    public class StockInResponseModel
    {
        public long Id { get; set; }
        public string InvoiceNumber { get; set; }
        public SupplierMapper Supplier { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public string? NguoiTao { get; set; }
        public string? NguoiCapNhat { get; set; }
    }
}
