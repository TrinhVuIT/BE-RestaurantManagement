using RestaurantManagement.Data.RequestModels.Order;
using RestaurantManagement.Data.ResponseModels.Order;
using RestaurantManagement.Data.ResponseModels;

namespace RestaurantManagement.Business.OrderServices.StockInService
{
    public interface IStockInService
    {
        Task<bool> CreateNew(StockInRequestModel model);
        Task<bool> Delete(long id);
        Task<bool> Update(long id, StockInRequestModel model);
        Task<StockInResponseModel?> GetById(long id);
        Task<BasePaginationResponseModel<StockInResponseModel>> GetPaged(GetPagedStockInRequestModel model);
    }
}
