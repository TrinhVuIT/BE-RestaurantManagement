using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Commons;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities;
using RestaurantManagement.Data.RequestModels.Order;
using RestaurantManagement.Data.ResponseModels;
using RestaurantManagement.Data.ResponseModels.Order;
using static RestaurantManagement.Commons.Constants;

namespace RestaurantManagement.Business.OrderServices.StockOutService
{
    public class StockOutService : IStockOutService
    {
        private readonly DataContext _context;
        public StockOutService(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateNew(StockOutRequestModel model)
        {
            var newStockOut = new StockOut()
            {
                Reason = model.Reason,
                TotalAmount = model.TotalAmount,
            };
            _context.StockOut.Add(newStockOut);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(long id)
        {
            var stockOut = await _context.StockOut.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
            if (stockOut != null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(id)));

            var stockOutDetails = await _context.StockOutDetail.Include(x => x.StockOut)
                .Where(x => !x.IsDeleted && x.StockOut.Id == id).ToListAsync();
            stockOutDetails.ForEach(x => x.IsDeleted = true);

            stockOut!.IsDeleted = true;

            _context.StockOutDetail.UpdateRange(stockOutDetails);
            _context.StockOut.Update(stockOut);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<StockOutResponseModel?> GetById(long id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BasePaginationResponseModel<StockOutResponseModel>> GetPaged(GetPagedStockOutRequestModel model)
        {
            try
            {
                var query = GetAll().OrderByDescending(x => x.NgayCapNhat).AsQueryable();

                if(!string.IsNullOrEmpty(model.Keyword))
                {
                    var key = model.Keyword.ToLower().Trim();
                    query = query.Where(e => EF.Functions.Like(e.Reason.ToLower(), key));
                }

                var totalItem = 0;
                if(model.PageSize > 0)
                {
                    query = query.ApplyPaging(model.PageNo, model.PageSize, out totalItem);
                }
                var result = query.ToList();
                return new BasePaginationResponseModel<StockOutResponseModel>(model.PageNo, model.PageSize, result, totalItem);

            }catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                throw;
            }
        }

        public async Task<bool> Update(long id, StockOutRequestModel model)
        {
            var stockOut = await _context.StockOut.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
            if (stockOut != null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(id)));

            stockOut!.Reason = model.Reason;
            stockOut.TotalAmount = model.TotalAmount;

            _context.StockOut.Update(stockOut);
            return await _context.SaveChangesAsync() > 0;
        }

        private IQueryable<StockOutResponseModel> GetAll()
        {
            return _context.StockOut.Where(x => !x.IsDeleted)
                .Select(x => new StockOutResponseModel
                {
                    Id = x.Id,
                    Reason = x.Reason,
                    TotalAmount = x.TotalAmount,
                    NgayTao = x.NgayTao,
                    NgayCapNhat = x.NgayCapNhat,
                    NguoiTao = x.NguoiTao,
                    NguoiCapNhat = x.NguoiCapNhat,
                }).AsQueryable();
        }
    }
}
