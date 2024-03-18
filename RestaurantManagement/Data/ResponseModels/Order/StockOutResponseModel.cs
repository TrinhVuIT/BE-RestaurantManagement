namespace RestaurantManagement.Data.ResponseModels.Order
{
    public class StockOutResponseModel
    {
        public long Id { get; set; }
        public string Reason { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public string? NguoiTao { get; set; }
        public string? NguoiCapNhat { get; set; }
    }
}
