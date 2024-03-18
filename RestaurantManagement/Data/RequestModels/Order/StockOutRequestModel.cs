namespace RestaurantManagement.Data.RequestModels.Order
{
    public class StockOutRequestModel
    {
        public string Reason { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
