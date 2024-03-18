using RestaurantManagement.Data.RequestModels.PurchaseOrder;
using RestaurantManagement.Data.ResponseModels;
using RestaurantManagement.Data.ResponseModels.PurchaseOrder;

namespace RestaurantManagement.Business.PurchaseOrderService
{
    public interface IPurchaseOrderService
    {
        Task<bool> CreateNew(PurchaseOrderRequestModel model);
        Task<bool> Delete(long id);
        Task<bool> Update(long id, PurchaseOrderRequestModel model);
        Task<PurchaseOrderResponseModel?> GetById(long id);
        Task<BasePaginationResponseModel<PurchaseOrderResponseModel>> GetPaged(GetPagedPurchaseOrderRequestModel model);
    }
}
