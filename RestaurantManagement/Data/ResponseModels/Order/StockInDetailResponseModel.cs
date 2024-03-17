using RestaurantManagement.Data.ResponseModels.FoodResponseModel;

namespace RestaurantManagement.Data.ResponseModels.Order
{
    public class StockInDetailResponseModel
    {
        public long Id { get; set; }
        public StockInMapper StockIn { get; set; }
        public IngredientMapper Ingredient { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public string? NguoiTao { get; set; }
        public string? NguoiCapNhat { get; set; }
    }

    public class StockInMapper
    {
        public long Id { get; set; }
        public string SupplierName { get; set; }
        public string? SupplierPhoneNumber { get; set;}
    }
}
