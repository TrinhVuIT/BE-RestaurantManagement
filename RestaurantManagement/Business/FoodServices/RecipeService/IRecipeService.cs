using RestaurantManagement.Data.RequestModels.Food;
using RestaurantManagement.Data.ResponseModels;
using RestaurantManagement.Data.ResponseModels.FoodResponseModel;

namespace RestaurantManagement.Business.FoodServices.RecipeService
{
    public interface IRecipeService
    {
        Task<bool> CreateNew(RecipeRequestModel model);
        Task<bool> Update(long id, RecipeRequestModel model);
        Task<bool> Delete(long id);
        Task<RecipeResponseModel?> GetById(long id);
        Task<BasePaginationResponseModel<RecipeResponseModel>> GetPaged(GetPagedRecipeRequestModel model);
    }
}
