using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Commons;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities;
using RestaurantManagement.Data.RequestModels.Order;
using RestaurantManagement.Data.ResponseModels;
using RestaurantManagement.Data.ResponseModels.FoodResponseModel;
using RestaurantManagement.Data.ResponseModels.Order;
using static RestaurantManagement.Commons.Constants;

namespace RestaurantManagement.Business.OrderServices.StockOutDetailService
{
    public class StockOutDetailService : IStockOutDetailService
    {
        private readonly DataContext _context;
        public StockOutDetailService(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateNew(StockOutDetailRequestModel model)
        {
            var stockOut = await _context.StockOut.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == model.StockOutId);
            if (stockOut == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(model.StockOutId)));

            var ingredient = await _context.Ingredient.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == model.IngredientId);
            if (ingredient == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(model.IngredientId)));

            var newStockOutDetail = new StockOutDetail()
            {
                StockOut = stockOut,
                Ingredient = ingredient,
                Quantity = model.Quantity,
                UnitPrice = model.UnitPrice,
            };
            _context.StockOutDetail.Add(newStockOutDetail);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(long id)
        {
            var stockOutDetail = await _context.StockOutDetail.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
            if (stockOutDetail == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(id)));

            stockOutDetail.IsDeleted = true;
            _context.StockOutDetail.Update(stockOutDetail);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<StockOutDetailResponseModel?> GetById(long id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BasePaginationResponseModel<StockOutDetailResponseModel>> GetPagedByStockoutId(GetPagedStockOutDetailRequestModel model)
        {
            try
            {
                var query = GetAll().Where(x => x.StockOut.Id == model.StockOutId).OrderByDescending(x => x.NgayCapNhat).AsQueryable();

                if(!string.IsNullOrEmpty(model.Keyword))
                {
                    var key = model.Keyword.ToLower().Trim();
                    query = query.Where(e => EF.Functions.Like(e.Ingredient.IngredientName, key));
                }

                var totalItem = 0;
                if(model.PageSize > 0)
                {
                    query = query.ApplyPaging(model.PageNo, model.PageSize, out totalItem);
                }

                var result = query.ToList();
                return new BasePaginationResponseModel<StockOutDetailResponseModel>(model.PageNo, model.PageSize, result, totalItem);

            }catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                throw;
            }
        }

        public async Task<bool> Update(long id, UpdateStockOutDetailRequestModel model)
        {
            var stockOutDetail = await _context.StockOutDetail.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
            if (stockOutDetail == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(id)));

            var ingredient = await _context.Ingredient.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == model.IngredientId);
            if (ingredient == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(model.IngredientId)));

            stockOutDetail.Ingredient = ingredient;
            stockOutDetail.Quantity = model.Quantity;
            stockOutDetail.UnitPrice = model.UnitPrice;

            _context.StockOutDetail.Update(stockOutDetail);
            return await _context.SaveChangesAsync() > 0;
        }
        private IQueryable<StockOutDetailResponseModel> GetAll()
        {
            return _context.StockOutDetail.Include(x => x.StockOut).Include(x => x.Ingredient)
                .Where(x => !x.IsDeleted).Select(x => new StockOutDetailResponseModel
                {
                    Id = x.Id,
                    StockOut = new StockOutMapper
                    {
                        Id = x.StockOut.Id,
                        Reason = x.StockOut.Reason,
                        TotalAmount = x.StockOut.TotalAmount,
                    },
                    Ingredient = new IngredientMapper
                    {
                        Id = x.Ingredient.Id,
                        IngredientName = x.Ingredient.IngredientName,
                        Exp = x.Ingredient.Exp,
                    },
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    NgayTao = x.NgayTao,
                    NgayCapNhat = x.NgayCapNhat,
                    NguoiTao = x.NguoiTao,
                    NguoiCapNhat = x.NguoiCapNhat,
                }).AsQueryable();
        }
    }
}
