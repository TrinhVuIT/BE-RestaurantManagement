using RestaurantManagement.Data.Entities;

namespace RestaurantManagement.Data.RequestModels.Order
{
    public class GetPagedStockInRequestModel : BasePaginationRequestModel
    {
        public string? InvoiceNumber { get; set; }
    }
}
