using RestaurantManagement.Data.Entities;
using RestaurantManagement.Data.RequestModels.Food;
using RestaurantManagement.Data.ResponseModels;

namespace RestaurantManagement.Business.FoodServices
{
    public interface IFoodService
    {
        Task<bool> Delete(long id);
        Task<bool> Update(long id, FoodRequestModel model);
        Task<bool> CreateNew(FoodRequestModel model);
        Task<Food> GetById(long id);
        Task<BasePaginationResponseModel<Food>> GetPaged(GetPageFoodRequestModel model);
    }
}
