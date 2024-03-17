namespace RestaurantManagement.Data.RequestModels.Order
{
    public class StockInDetailRequestModel
    {
        public long StockInId { get; set; }
        public long IngredientId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
    public class UpdateStockInDetailRequestModel
    {
        public long IngredientId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
