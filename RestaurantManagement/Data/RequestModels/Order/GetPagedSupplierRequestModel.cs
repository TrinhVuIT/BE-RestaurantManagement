namespace RestaurantManagement.Data.RequestModels.Order
{
    public class GetPagedSupplierRequestModel : BasePaginationRequestModel
    {
        public string? PhoneNumber { get; set; }
    }
}
