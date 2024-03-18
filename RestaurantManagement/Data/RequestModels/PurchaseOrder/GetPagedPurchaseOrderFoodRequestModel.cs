namespace RestaurantManagement.Data.RequestModels.PurchaseOrder
{
    public class GetPagedPurchaseOrderFoodRequestModel : BasePaginationRequestModel
    {
        public long PurchaseOrderId { get; set; }
    }
}
