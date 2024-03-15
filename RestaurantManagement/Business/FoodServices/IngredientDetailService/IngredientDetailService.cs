using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Commons;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities;
using RestaurantManagement.Data.RequestModels.Food;
using RestaurantManagement.Data.ResponseModels;
using RestaurantManagement.Data.ResponseModels.FoodResponseModel;
using static RestaurantManagement.Commons.Constants;

namespace RestaurantManagement.Business.FoodServices.IngredientDetailService
{
    public class IngredientDetailService : IIngredientDetailService
    {
        private readonly DataContext _context;
        public IngredientDetailService(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateNew(IngredientDetailRequestModel model)
        {
            var recipe = await _context.Recipe.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == model.RecipeId);
            if (recipe == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(model.RecipeId)));

            var ingredient = await _context.Ingredient.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == model.IngredientId);
            if (ingredient == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(model.IngredientId)));
            var res = new IngredientDetail()
            {
                Recipe = recipe,
                Ingredient = ingredient,
                Quantity = model.Quantity,
                Unit = model.Unit
            };
            _context.IngredientDetail.Add(res);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(long id)
        {
            var res = await _context.IngredientDetail.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
            if (res == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(id)));
            res.IsDeleted = true;
            _context.IngredientDetail.Update(res);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(long id, UpdateIngredientDetailRequestModel model)
        {
            var res = await _context.IngredientDetail.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
            if (res == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(id)));

            var ingredient = await _context.Ingredient.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == model.IngredientId);
            if (ingredient == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(model.IngredientId)));

            res.Ingredient = ingredient;
            res.Quantity = model.Quantity;
            res.Unit = model.Unit;

            _context.IngredientDetail.Update(res);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IngredientDetailResponseModel?> GetById(long id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BasePaginationResponseModel<IngredientDetailResponseModel>> GetPagedByRecipeId(GetPagedIngredientDetailRequestModel model)
        {
            try
            {
                var query = GetAll().Where(x => x.Recipe.Id == model.RecipeId).OrderByDescending(x => x.NgayCapNhat).AsQueryable();
                query = ApplySearch(query, model);

                var totalItem = 0;
                if(model.PageSize > 0)
                {
                    query = query.ApplyPaging(model.PageNo, model.PageSize, out totalItem);
                }
                List<IngredientDetailResponseModel> result = query.ToList();
                return new BasePaginationResponseModel<IngredientDetailResponseModel>(model.PageNo, model.PageSize, result, totalItem);

            }catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                throw;
            }
        }

        private IQueryable<IngredientDetailResponseModel> ApplySearch(IQueryable<IngredientDetailResponseModel> query, GetPagedIngredientDetailRequestModel model)
        {
            if(!string.IsNullOrEmpty(model.Keyword))
            {
                var key = model.Keyword.ToLower().Trim();
                query = query.Where(e => EF.Functions.Like(e.Ingredient.IngredientName.ToLower(), key));
            }

            if(model.Exp != null)
            {
                query = query.Where(e => e.Ingredient.Exp == model.Exp);
            }
            return query;
        }

        private IQueryable<IngredientDetailResponseModel> GetAll()
        {
            return _context.IngredientDetail.Include(x => x.Recipe).Include(x => x.Ingredient)
                .Where(x => !x.IsDeleted).Select(x => new IngredientDetailResponseModel
                {
                    Id = x.Id,
                    Recipe = new RecipeMapper
                    {
                        Id = x.Recipe.Id,
                        RecipeName = x.Recipe.RecipeName,
                        Description = x.Recipe.Description,
                        Step = x.Recipe.Step,
                    },
                    Ingredient = new IngredientMapper
                    {
                        Id = x.Ingredient.Id,
                        IngredientName = x.Ingredient.IngredientName,
                        Exp = x.Ingredient.Exp,
                    },
                    Quantity = x.Quantity,
                    Unit = x.Unit,
                    NgayTao = x.NgayTao,
                    NgayCapNhat = x.NgayCapNhat,
                    NguoiTao = x.NguoiTao,
                    NguoiCapNhat = x.NguoiCapNhat,
                }).AsQueryable();
        }
    }
}
