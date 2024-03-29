using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Commons;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities;
using RestaurantManagement.Data.RequestModels.Food;
using RestaurantManagement.Data.ResponseModels;

namespace RestaurantManagement.Business.FoodServices
{
    public class FoodService : IFoodService
    {
        public readonly DataContext _context;
        public FoodService(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateNew(FoodRequestModel model)
        {
            var newFood = new Food
            {
                FoodName = model.FoodName,
                FoodDescription = model.FoodDescription,
                FoodPrice = model.FoodPrice,
            };
            await _context.Food.AddAsync(newFood);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(long id)
        {
            var food = await GetById(id);
            if (food == null)
                throw new Exception(string.Format(Constants.ExceptionMessage.NOT_FOUND, nameof(id)));

            var recipeDelete = await _context.Recipe.Where(x => !x.IsDeleted && x.Id == id).ToListAsync();
            if (recipeDelete != null && recipeDelete.Count > 0)
            {
                foreach (var item in recipeDelete)
                {
                    var ingredientDetail = _context.IngredientDetail.Where(x => !x.IsDeleted && x.Recipe.Id == item.Id).ToList();
                    ingredientDetail.ForEach(x => x.IsDeleted = true);
                    _context.IngredientDetail.UpdateRange(ingredientDetail);
                }
                recipeDelete.ForEach(x => x.IsDeleted = true);
                _context.Recipe.UpdateRange(recipeDelete);
            }

            food.IsDeleted = true;
            _context.Food.Update(food);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> Update(long id, FoodRequestModel model)
        {
            var updateFood = await GetById(id);
            if (updateFood == null)
                throw new Exception(string.Format(Constants.ExceptionMessage.NOT_FOUND, nameof(id)));

            updateFood.FoodName = model.FoodName;
            updateFood.FoodDescription = model.FoodDescription;
            updateFood.FoodPrice = model.FoodPrice;

            _context.Food.Update(updateFood);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Food?> GetById(long id)
        {
            var result = await _context.Food.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            return result;
        }

        public async Task<BasePaginationResponseModel<Food>> GetPaged(GetPageFoodRequestModel model)
        {
            try
            {
                var query = _context.Food.Where(x => !x.IsDeleted).AsQueryable();
                //Apply order
                query = query.OrderByDescending(x => x.NgayTao);
                //Search
                if (!string.IsNullOrEmpty(model.Keyword))
                {
                   var keyword = model.Keyword.ToLower().Trim();
                    query = query.Where(x => EF.Functions.Like(x.FoodName.ToLower(),keyword));
                }

                var totalItems = 0;
                if(model.PageSize > 0)
                {
                    query = query.ApplyPaging(model.PageNo, model.PageSize, out totalItems);
                }
                List<Food> result = query.ToList();
                return new BasePaginationResponseModel<Food>(model.PageNo, model.PageSize, result, totalItems);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                throw;
            }
        }

    }
}
