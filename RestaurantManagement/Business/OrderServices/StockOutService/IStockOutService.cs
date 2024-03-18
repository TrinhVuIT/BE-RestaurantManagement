using RestaurantManagement.Data.RequestModels.Order;
using RestaurantManagement.Data.ResponseModels.Order;
using RestaurantManagement.Data.ResponseModels;

namespace RestaurantManagement.Business.OrderServices.StockOutService
{
    public interface IStockOutService
    {
        Task<bool> CreateNew(StockOutRequestModel model);
        Task<bool> Delete(long id);
        Task<bool> Update(long id, StockOutRequestModel model);
        Task<StockOutResponseModel?> GetById(long id);
        Task<BasePaginationResponseModel<StockOutResponseModel>> GetPaged(GetPagedStockOutRequestModel model);
    }
}
