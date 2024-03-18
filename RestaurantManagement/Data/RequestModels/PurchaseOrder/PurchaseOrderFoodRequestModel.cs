namespace RestaurantManagement.Data.RequestModels.PurchaseOrder
{
    public class PurchaseOrderFoodRequestModel
    {
        public long PurchaseOrderId { get; set; }
        public long FoodId { get; set; }
        public int Quantity { get; set; }
    }
    public class UpdatePurchaseOrderFoodRequestModel
    {
        public long FoodId { get; set; }
        public int Quantity { get; set; }
    }
}
