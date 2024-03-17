using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Commons;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities;
using RestaurantManagement.Data.RequestModels.Food;
using RestaurantManagement.Data.ResponseModels;
using RestaurantManagement.Data.ResponseModels.FoodResponseModel;
using static RestaurantManagement.Commons.Constants;

namespace RestaurantManagement.Business.FoodServices.RecipeService
{
    public class RecipeService : IRecipeService
    {
        private readonly DataContext _context;
        public RecipeService(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateNew(RecipeRequestModel model)
        {
            var food = await _context.Food.FirstOrDefaultAsync(r => !r.IsDeleted && r.Id == model.FoodId); ;
            if (food == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(model.FoodId)));

            var newRecipe = new Recipe()
            {
                Food = food,
                RecipeName = model.RecipeName,
                Step = model.Step,
                Description = model.Description,
            };
            _context.Recipe.Add(newRecipe);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(long id)
        {
            var res = await _context.Recipe.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
            if (res == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(id)));
            var ingredientDetail = await _context.IngredientDetail.Include(x => x.Ingredient)
                .Where(x => !x.IsDeleted && x.Ingredient.Id == id).ToListAsync();
            ingredientDetail.ForEach(x => x.IsDeleted = true);

            res.IsDeleted = true;
            _context.IngredientDetail.UpdateRange(ingredientDetail);
            _context.Recipe.Update(res);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<RecipeResponseModel?> GetById(long id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BasePaginationResponseModel<RecipeResponseModel>> GetPaged(GetPagedRecipeRequestModel model)
        {
            try
            {
                var query = GetAll();
                query = query.OrderByDescending(x => x.NgayCapNhat);
                query = ApplySearch(query, model);

                var totalItem = 0;
                if (model.PageSize > 0)
                {
                    query = query.ApplyPaging(model.PageNo, model.PageSize, out totalItem);
                }
                List<RecipeResponseModel> result = query.ToList();
                return new BasePaginationResponseModel<RecipeResponseModel>(model.PageNo, model.PageSize, result, totalItem);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                throw;
            }
        }

        public async Task<bool> Update(long id, RecipeRequestModel model)
        {
            var updateRecipe = await _context.Recipe.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (updateRecipe == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(id)));

            var food = await _context.Food.FirstOrDefaultAsync(r => !r.IsDeleted && r.Id == model.FoodId);
            if (food == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(model.FoodId)));

            updateRecipe.Food = food;
            updateRecipe.RecipeName = model.RecipeName;
            updateRecipe.Step = model.Step;
            updateRecipe.Description = model.Description;

            _context.Recipe.Update(updateRecipe);
            return await _context.SaveChangesAsync() > 0;
        }

        private IQueryable<RecipeResponseModel> ApplySearch(IQueryable<RecipeResponseModel> query, GetPagedRecipeRequestModel model)
        {
            if (!string.IsNullOrEmpty(model.Keyword))
            {
                var keyword = model.Keyword.ToLower().Trim();
                query = query.Where(re => EF.Functions.Like(re.RecipeName.ToLower(), keyword));
            }

            if (!string.IsNullOrEmpty(model.FoodName))
            {
                var keyword = model.FoodName.ToLower().Trim();
                query = query.Where(re => EF.Functions.Like(re.Food.FoodName.ToLower(), keyword));
            }
            return query;
        }

        private IQueryable<RecipeResponseModel> GetAll()
        {
            return _context.Recipe.Include(x => x.Food)
                .Where(x => !x.IsDeleted)
                .Select(x => new RecipeResponseModel
                {
                    Id = x.Id,
                    RecipeName = x.RecipeName,
                    Food = new FoodMapper
                    {
                        Id = x.Food.Id,
                        FoodName = x.Food.FoodName,
                        FoodDescription = x.Food.FoodDescription,
                        FoodPrice = x.Food.FoodPrice,
                    },
                    IngredientDetail = _context.IngredientDetail
                        .Include(r => r.Recipe).Include(r => r.Ingredient)
                        .Where(r => r.Recipe.Id == x.Id)
                        .Select(r => new IngredientDetailMapper
                        {
                            Id = r.Id,
                            IngredientName = r.Ingredient.IngredientName,
                            Exp = r.Ingredient.Exp,
                            Quantity = r.Quantity,
                            Unit = r.Unit
                        }).ToList(),
                    Step = x.Step,
                    Description = x.Description,
                    NgayTao = x.NgayTao,
                    NgayCapNhat = x.NgayCapNhat,
                    NguoiTao = x.NguoiTao,
                    NguoiCapNhat = x.NguoiCapNhat,
                }).AsQueryable();
        }
    }
}
