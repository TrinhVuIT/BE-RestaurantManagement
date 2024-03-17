using RestaurantManagement.Data.RequestModels.Order;
using RestaurantManagement.Data.ResponseModels.Order;
using RestaurantManagement.Data.ResponseModels;

namespace RestaurantManagement.Business.OrderServices.StockInDetailService
{
    public interface IStockInDetailService
    {
        Task<bool> CreateNew(StockInDetailRequestModel model);
        Task<bool> Delete(long id);
        Task<bool> Update(long id, UpdateStockInDetailRequestModel model);
        Task<StockInDetailResponseModel?> GetById(long id);
        Task<BasePaginationResponseModel<StockInDetailResponseModel>> GetPagedByStockInId(GetPagedStockInDetailRequestModel model);
    }
}
