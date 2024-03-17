using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Commons;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities;
using RestaurantManagement.Data.RequestModels.Order;
using RestaurantManagement.Data.ResponseModels;
using static RestaurantManagement.Commons.Constants;

namespace RestaurantManagement.Business.OrderServices.SupplierService
{
    public class SupplierService : ISupplierService
    {
        private readonly DataContext _context;
        public SupplierService(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateNew(SupplierRequestModel model)
        {
            var res = new Supplier()
            {
                SupplierName = model.SupplierName,
                SupplierAddress = model.SupplierAddress,
                PhoneNumber = model.PhoneNumber,
            };
            _context.Supplier.Add(res);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(long id)
        {
            var res = await GetById(id);
            if(res == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(id)));

            var order = await _context.Order.Include(x => x.Supplier).Where(x => !x.IsDeleted && x.Supplier.Id == id).ToListAsync();
            order.ForEach(x => x.IsDeleted = true);

            res.IsDeleted = true;

            _context.Order.UpdateRange(order);
            _context.Supplier.Update(res);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Supplier?> GetById(long id)
        {
            return await _context.Supplier.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
        }

        public async Task<BasePaginationResponseModel<Supplier>> GetPaged(GetPagedSupplierRequestModel model)
        {
            try
            {
                var query = _context.Supplier.Where(x => !x.IsDeleted).AsQueryable();

                query = query.OrderByDescending(x => x.NgayCapNhat);

                if (!string.IsNullOrEmpty(model.Keyword))
                {
                    var key = model.Keyword.ToLower().Trim();
                    query = query.Where(e => EF.Functions.Like(e.SupplierName.ToLower(), key));
                }

                if (!string.IsNullOrEmpty(model.PhoneNumber))
                {
                    var key = model.PhoneNumber.ToLower().Trim();
                    query = query.Where(e => EF.Functions.Like(e.PhoneNumber, key));
                }

                var totalItem = 0;
                if(model.PageSize > 0)
                {
                    query = query.ApplyPaging(model.PageNo, model.PageSize, out totalItem);
                }
                var result = query.ToList();
                return new BasePaginationResponseModel<Supplier>(model.PageNo, model.PageSize, result, totalItem);

            }catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                throw;
            }
        }

        public async Task<bool> Update(long id, SupplierRequestModel model)
        {
            var res = await GetById(id);
            if (res == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(id)));

            res.SupplierName = model.SupplierName;
            res.SupplierAddress = model.SupplierAddress;
            res.PhoneNumber = model.PhoneNumber;

            _context.Supplier.Update(res);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
