using RestaurantManagement.Data.RequestModels.Food;
using RestaurantManagement.Data.ResponseModels;
using RestaurantManagement.Data.ResponseModels.FoodResponseModel;

namespace RestaurantManagement.Business.FoodServices.IngredientDetailService
{
    public interface IIngredientDetailService
    {
        Task<bool> CreateNew(IngredientDetailRequestModel model);
        Task<bool> Update(long id, UpdateIngredientDetailRequestModel model);
        Task<bool> Delete(long id);
        Task<IngredientDetailResponseModel?> GetById(long id);
        Task<BasePaginationResponseModel<IngredientDetailResponseModel>> GetPagedByRecipeId(GetPagedIngredientDetailRequestModel model);
    }
}
