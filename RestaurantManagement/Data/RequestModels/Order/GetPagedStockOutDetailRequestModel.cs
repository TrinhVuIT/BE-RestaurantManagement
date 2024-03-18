namespace RestaurantManagement.Data.RequestModels.Order
{
    public class GetPagedStockOutDetailRequestModel : BasePaginationRequestModel
    {
        public long StockOutId { get; set; }
    }
}
