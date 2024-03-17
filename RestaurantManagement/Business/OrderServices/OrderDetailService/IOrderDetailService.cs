using RestaurantManagement.Data.RequestModels.Order;
using RestaurantManagement.Data.ResponseModels.Order;
using RestaurantManagement.Data.ResponseModels;

namespace RestaurantManagement.Business.OrderServices.OrderDetailService
{
    public interface IOrderDetailService
    {
        Task<bool> CreateNew(OrderDetailRequestModel model);
        Task<bool> Delete(long id);
        Task<bool> Update(long id, UpdateOrderDetailRequestModel model);
        Task<OrderDetailReponseModel?> GetById(long id);
        Task<BasePaginationResponseModel<OrderDetailReponseModel>> GetPagedByOrderId(GetPagedOrderDetailRequestModel model);
    }
}
