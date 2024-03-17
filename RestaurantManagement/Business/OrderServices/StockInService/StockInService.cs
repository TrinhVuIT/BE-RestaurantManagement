using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Commons;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities;
using RestaurantManagement.Data.RequestModels.Order;
using RestaurantManagement.Data.ResponseModels;
using RestaurantManagement.Data.ResponseModels.Order;
using static RestaurantManagement.Commons.Constants;

namespace RestaurantManagement.Business.OrderServices.StockInService
{
    public class StockInService : IStockInService
    {
        private readonly DataContext _context;
        public StockInService(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateNew(StockInRequestModel model)
        {
            var supplier = await _context.Supplier.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == model.SupplierId);
            if (supplier == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(model.SupplierId)));

            var newStockIn = new StockIn()
            {
                InvoiceNumber = model.InvoiceNumber,
                Supplier = supplier,
                TotalAmount = model.TotalAmount,
            };

            _context.StockIn.Add(newStockIn);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(long id)
        {
            var stockIn = await _context.StockIn.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
            if (stockIn == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(id)));

            stockIn.IsDeleted = true;
            _context.StockIn.Update(stockIn);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<StockInResponseModel?> GetById(long id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BasePaginationResponseModel<StockInResponseModel>> GetPaged(GetPagedStockInRequestModel model)
        {
            try
            {
                var query = GetAll().OrderByDescending(x => x.NgayCapNhat).AsQueryable();

                if (!string.IsNullOrEmpty(model.Keyword))
                {
                    var key = model.Keyword.ToLower().Trim();
                    query = query.Where(e => EF.Functions.Like(e.Supplier.SupplierName, key));
                }

                if (!string.IsNullOrEmpty(model.InvoiceNumber))
                {
                    var key = model.InvoiceNumber.ToLower().Trim();
                    query = query.Where(e => EF.Functions.Like(e.InvoiceNumber, key));
                }

                var totalItem = 0;
                if (model.PageSize > 0)
                {
                    query = query.ApplyPaging(model.PageNo, model.PageSize, out totalItem);
                }

                List<StockInResponseModel> result = query.ToList();
                return new BasePaginationResponseModel<StockInResponseModel>(model.PageNo, model.PageSize, result, totalItem);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                throw;
            }
        }

        public async Task<bool> Update(long id, StockInRequestModel model)
        {
            var stockIn = await _context.StockIn.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
            if (stockIn == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(id)));

            var supplier = await _context.Supplier.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == model.SupplierId);
            if (supplier == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(model.SupplierId)));

            stockIn.Supplier = supplier;
            stockIn.InvoiceNumber = model.InvoiceNumber;
            stockIn.TotalAmount = model.TotalAmount;

            _context.StockIn.Update(stockIn);
            return await _context.SaveChangesAsync() > 0;
        }

        private IQueryable<StockInResponseModel> GetAll()
        {
            return _context.StockIn.Include(x => x.Supplier)
                .Where(x => !x.IsDeleted).Select(x => new StockInResponseModel
                {
                    Id = x.Id,
                    InvoiceNumber = x.InvoiceNumber,
                    TotalAmount = x.TotalAmount,
                    Supplier = new SupplierMapper
                    {
                        Id = x.Supplier.Id,
                        SupplierName = x.Supplier.SupplierName,
                        SupplierAddress = x.Supplier.SupplierAddress,
                        PhoneNumber = x.Supplier.PhoneNumber,
                    },
                    NgayTao = x.NgayTao,
                    NgayCapNhat = x.NgayCapNhat,
                    NguoiTao = x.NguoiTao,
                    NguoiCapNhat = x.NguoiCapNhat,
                }).AsQueryable();
        }
    }
}
