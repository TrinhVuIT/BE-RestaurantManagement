using static RestaurantManagement.Commons.Enums;

namespace RestaurantManagement.Data.RequestModels.PurchaseOrder
{
    public class GetPagedPurchaseOrderRequestModel : BasePaginationRequestModel
    {
        public StatusOrder? Status { get; set; }
    }
}
