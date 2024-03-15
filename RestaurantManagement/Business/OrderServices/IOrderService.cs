using RestaurantManagement.Data.RequestModels.Order;
using RestaurantManagement.Data.ResponseModels;
using RestaurantManagement.Data.ResponseModels.Order;

namespace RestaurantManagement.Business.OrderServices
{
    public interface IOrderService
    {
        Task<bool> CreateNew(OrderRequestModel model);
        Task<bool> Delete(long id);
        Task<bool> Update(OrderRequestModel model);
        Task<OrderResponseModel> GetById(long id);
        Task<BasePaginationResponseModel<OrderResponseModel>> GetPaged(GetPagedOrderRequestModel model);
    }
}
