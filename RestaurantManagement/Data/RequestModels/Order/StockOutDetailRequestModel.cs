namespace RestaurantManagement.Data.RequestModels.Order
{
    public class StockOutDetailRequestModel
    {
        public long StockOutId { get; set; }
        public long IngredientId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
    public class UpdateStockOutDetailRequestModel
    {
        public long IngredientId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
