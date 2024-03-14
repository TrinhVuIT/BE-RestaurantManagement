using RestaurantManagement.Data.RequestModels.Food;
using RestaurantManagement.Data.ResponseModels;
using RestaurantManagement.Data.ResponseModels.FoodResponseModel;

namespace RestaurantManagement.Business.FoodServices.RecipeService
{
    public class RecipeService : IRecipeService
    {
        public Task<bool> CreateNew(CreateRecipeRequestModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Task<RecipeResponseModel> GetById(long id)
        {
            throw new NotImplementedException();
        }

        public Task<BasePaginationResponseModel<RecipeResponseModel>> GetPaged(GetPagedRecipeRequestModel model)
        {
            throw new NotImplementedException();
        }
    }
}
