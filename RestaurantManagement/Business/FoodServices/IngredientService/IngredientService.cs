using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Commons;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities;
using RestaurantManagement.Data.RequestModels.Food;
using RestaurantManagement.Data.ResponseModels;

namespace RestaurantManagement.Business.FoodServices.IngredientService
{
    public class IngredientService : IIngredientService
    {
        private readonly DataContext _context;
        public IngredientService(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateNew(IngredientRequestModel model)
        {
            var newIngredient = new Ingredient()
            {
                IngredientName = model.IgredientName,
                Exp = model.Exp
            };
            _context.Add(newIngredient);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(long id)
        {
            var res = await GetById(id);
            if(res ==  null)
                throw new Exception(string.Format(Constants.ExceptionMessage.FAILED, nameof(id)));
            res.IsDeleted = true;
            _context.Update(res);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Ingredient> GetById(long id)
        {
            return await _context.Ingredient.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
        }

        public async Task<BasePaginationResponseModel<Ingredient>> GetPaged(GetPagedIngredientRequestModel model)
        {
            try
            {
                var query = _context.Ingredient.Where(x => !x.IsDeleted).AsQueryable();
                query = query.OrderByDescending(x => x.NgayTao);

                if (!string.IsNullOrEmpty(model.Keyword))
                {
                    var keyword = model.Keyword.ToLower().Trim();
                    query = query.Where(e => EF.Functions.Like(e.IngredientName.ToLower(), keyword));
                }
                if(model.Exp.HasValue)
                {
                    query = query.Where(x => x.Exp == model.Exp.Value);
                }

                var totalItem = 0;
                if(model.PageSize > 0)
                {
                    query = query.ApplyPaging(model.PageNo, model.PageSize, out totalItem);
                }
                List<Ingredient> result = query.ToList();
                return new BasePaginationResponseModel<Ingredient>(model.PageNo, model.PageSize, result, totalItem);

            }catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                throw;
            }
        }

        public async Task<bool> Update(long id, IngredientRequestModel model)
        {
            var res = await GetById(id);
            if(res == null)
                throw new Exception(string.Format(Constants.ExceptionMessage.FAILED, nameof(id)));
            res.IngredientName = model.IgredientName;
            res.Exp = model.Exp;

            _context.Update(res);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
