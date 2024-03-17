namespace RestaurantManagement.Data.ResponseModels.Order
{
    public class OrderDetailRequestModel
    {
        public long OrderId { get; set; }
        public long IngredientId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateOrderDetailRequestModel
    {
        public long IngredientId { get; set; }
        public int Quantity { get; set; }
    }
}
