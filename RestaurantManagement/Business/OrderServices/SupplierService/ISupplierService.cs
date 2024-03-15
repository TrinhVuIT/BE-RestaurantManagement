using RestaurantManagement.Data.Entities;
using RestaurantManagement.Data.RequestModels.Order;
using RestaurantManagement.Data.ResponseModels;

namespace RestaurantManagement.Business.OrderServices.SupplierService
{
    public interface ISupplierService
    {
        Task<bool> CreateNew(SupplierRequestModel model);
        Task<bool> Delete(long id);
        Task<bool> Update(long id, SupplierRequestModel model);
        Task<Supplier?> GetById(long id);
        Task<BasePaginationResponseModel<Supplier>> GetPaged(GetPagedSupplierRequestModel model);
    }
}
