namespace RestaurantManagement.Data.RequestModels.Order
{
    public class GetPagedOrderDetailRequestModel : BasePaginationRequestModel
    {
        public long OrderId { get; set; }
    }
}
