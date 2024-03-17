namespace RestaurantManagement.Data.RequestModels.Order
{
    public class GetPagedStockInDetailRequestModel : BasePaginationRequestModel
    {
        public long StockInId { get; set; }
    }
}
