using RestaurantManagement.Data.Entities;
using RestaurantManagement.Data.RequestModels.Food;
using RestaurantManagement.Data.ResponseModels;

namespace RestaurantManagement.Business.FoodServices.IngredientService
{
    public interface IIngredientService
    {
        Task<bool> CreateNew(IngredientRequestModel model);
        Task<bool> Delete(long id);
        Task<bool> Update(long id, IngredientRequestModel model);
        Task<Ingredient> GetById(long id);
        Task<BasePaginationResponseModel<Ingredient>> GetPaged(GetPagedIngredientRequestModel model);
    }
}
