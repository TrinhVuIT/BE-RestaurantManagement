using RestaurantManagement.Data.RequestModels.PurchaseOrder;
using RestaurantManagement.Data.ResponseModels.PurchaseOrder;
using RestaurantManagement.Data.ResponseModels;

namespace RestaurantManagement.Business.PurchaseOrderService.PurchaseOrderFoodService
{
    public interface IPurchaseOrderFoodService
    {
        Task<bool> CreateNew(PurchaseOrderFoodRequestModel model);
        Task<bool> Delete(long id);
        Task<bool> Update(long id, UpdatePurchaseOrderFoodRequestModel model);
        Task<PurchaseOrderFoodResponseModel?> GetById(long id);
        Task<BasePaginationResponseModel<PurchaseOrderFoodResponseModel>> GetPagedByPurchaseOrderId(GetPagedPurchaseOrderFoodRequestModel model);
    }
}
