using static RestaurantManagement.Commons.Enums;

namespace RestaurantManagement.Data.RequestModels.Order
{
    public class GetPagedOrderRequestModel : BasePaginationRequestModel
    {
        public DateTime? DeliveryAppointment { get; set; }
        public StatusOrder? StatusOrder { get; set; }
    }
}
