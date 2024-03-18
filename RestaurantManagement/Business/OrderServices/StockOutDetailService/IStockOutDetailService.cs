using RestaurantManagement.Data.RequestModels.Order;
using RestaurantManagement.Data.ResponseModels.Order;
using RestaurantManagement.Data.ResponseModels;

namespace RestaurantManagement.Business.OrderServices.StockOutDetailService
{
    public interface IStockOutDetailService
    {
        Task<bool> CreateNew(StockOutDetailRequestModel model);
        Task<bool> Delete(long id);
        Task<bool> Update(long id, UpdateStockOutDetailRequestModel model);
        Task<StockOutDetailResponseModel?> GetById(long id);
        Task<BasePaginationResponseModel<StockOutDetailResponseModel>> GetPagedByStockoutId(GetPagedStockOutDetailRequestModel model);
    }
}
