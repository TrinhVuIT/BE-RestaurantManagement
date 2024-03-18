using RestaurantManagement.Data.ResponseModels.FoodResponseModel;

namespace RestaurantManagement.Data.ResponseModels.Order
{
    public class StockOutDetailResponseModel
    {
        public long Id { get; set; }
        public StockOutMapper StockOut { get; set; }
        public IngredientMapper Ingredient { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public string? NguoiTao { get; set; }
        public string? NguoiCapNhat { get; set; }
    }

    public class StockOutMapper
    {
        public long Id { get; set; }
        public string Reason { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
