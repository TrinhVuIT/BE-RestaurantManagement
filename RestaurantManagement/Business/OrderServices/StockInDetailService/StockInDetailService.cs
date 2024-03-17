using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Commons;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities;
using RestaurantManagement.Data.RequestModels.Order;
using RestaurantManagement.Data.ResponseModels;
using RestaurantManagement.Data.ResponseModels.FoodResponseModel;
using RestaurantManagement.Data.ResponseModels.Order;
using static RestaurantManagement.Commons.Constants;

namespace RestaurantManagement.Business.OrderServices.StockInDetailService
{
    public class StockInDetailService : IStockInDetailService
    {
        private readonly DataContext _context;
        public StockInDetailService(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateNew(StockInDetailRequestModel model)
        {
            var stockIn = await _context.StockIn.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == model.StockInId);
            if (stockIn == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(model.StockInId)));

            var ingredient = await _context.Ingredient.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == model.IngredientId);
            if (ingredient == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(model.IngredientId)));

            var newStockInDetail = new StockInDetail()
            {
                StockIn = stockIn,
                Ingredient = ingredient,
                Quantity = model.Quantity,
                UnitPrice = model.UnitPrice,
            };

            _context.StockInDetail.Add(newStockInDetail);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(long id)
        {
            var stockInDetail = await _context.StockInDetail.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
            if (stockInDetail == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(id)));

            stockInDetail.IsDeleted = true;
            _context.StockInDetail.Update(stockInDetail);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<StockInDetailResponseModel?> GetById(long id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BasePaginationResponseModel<StockInDetailResponseModel>> GetPagedByStockInId(GetPagedStockInDetailRequestModel model)
        {
            try
            {
                var query = GetAll().Where(x => x.StockIn.Id == model.StockInId).OrderByDescending(x => x.NgayCapNhat).AsQueryable();

                if (!string.IsNullOrEmpty(model.Keyword))
                {
                    var key = model.Keyword.ToLower().Trim();
                    query = query.Where(e => EF.Functions.Like(e.Ingredient.IngredientName, key));
                }

                var totalItem = 0;
                if(model.PageSize > 0)
                {
                    query = query.ApplyPaging(model.PageNo, model.PageSize, out totalItem);
                }
                List<StockInDetailResponseModel> result = query.ToList();
                return new BasePaginationResponseModel<StockInDetailResponseModel>(model.PageNo, model.PageSize, result, totalItem);

            }catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                throw;
            }
        }

        public async Task<bool> Update(long id, UpdateStockInDetailRequestModel model)
        {
            var stockInDetail = await _context.StockInDetail.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
            if (stockInDetail == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(id)));

            var ingredient = await _context.Ingredient.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == model.IngredientId);
            if (ingredient == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(model.IngredientId)));

            stockInDetail.Ingredient = ingredient;
            stockInDetail.Quantity = model.Quantity;
            stockInDetail.UnitPrice = model.UnitPrice;

            _context.StockInDetail.Update(stockInDetail);
            return await _context.SaveChangesAsync() > 0;
        }

        private IQueryable<StockInDetailResponseModel> GetAll()
        {
            return _context.StockInDetail.Include(x => x.StockIn).ThenInclude(s => s.Supplier).Include(x => x.Ingredient)
                .Where(x => !x.IsDeleted).Select(x => new StockInDetailResponseModel
                {
                    Id = x.Id,
                    StockIn = new StockInMapper
                    {
                        Id = x.StockIn.Id,
                        SupplierName = x.StockIn.Supplier.SupplierName,
                        SupplierPhoneNumber = x.StockIn.Supplier.PhoneNumber,
                    },
                    Ingredient = new IngredientMapper
                    {
                        Id = x.Ingredient.Id,
                        IngredientName = x.Ingredient.IngredientName,
                        Exp = x.Ingredient.Exp
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
