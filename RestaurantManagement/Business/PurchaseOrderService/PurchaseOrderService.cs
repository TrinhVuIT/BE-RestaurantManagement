using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Commons;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities;
using RestaurantManagement.Data.RequestModels.PurchaseOrder;
using RestaurantManagement.Data.ResponseModels;
using RestaurantManagement.Data.ResponseModels.PurchaseOrder;
using static RestaurantManagement.Commons.Constants;

namespace RestaurantManagement.Business.PurchaseOrderService
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly DataContext _context;
        public PurchaseOrderService(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateNew(PurchaseOrderRequestModel model)
        {
            var customer = await _context.ApplicationUser.FirstOrDefaultAsync(x => !x.IsDeleted && x.IsActive && x.Id == model.CustomerId);

            var newPurchaseOrder = new PurchaseOrder()
            {
                Customer = customer,
                CustomerOther = model.CustomerOther,
                AddressCustomerOther = model.AddressCustomerOther,
                Status = Enums.StatusOrder.Pending,
                TotalPrice = 0
            };

            _context.PurchaseOrder.Add(newPurchaseOrder);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(long id)
        {
            var purchaseOrder = await _context.PurchaseOrder.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
            if (purchaseOrder != null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(id)));

            var purchaseOrderFood = await _context.PurchaseOrderFood.Include(p => p.PurchaseOrder)
                .Where(x => !x.IsDeleted && x.PurchaseOrder.Id == id).ToListAsync();

            purchaseOrderFood.ForEach(x => x.IsDeleted = true);
            purchaseOrder!.IsDeleted = true;

            _context.PurchaseOrderFood.UpdateRange(purchaseOrderFood);
            _context.PurchaseOrder.Update(purchaseOrder);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<PurchaseOrderResponseModel?> GetById(long id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BasePaginationResponseModel<PurchaseOrderResponseModel>> GetPaged(GetPagedPurchaseOrderRequestModel model)
        {
            try
            {
                var query = GetAll().OrderByDescending(x => x.NgayCapNhat).AsQueryable();

                if (!string.IsNullOrEmpty(model.Keyword))
                {
                    var key = model.Keyword.ToLower().Trim();
                    query = query.Where(e => e.Customer != null ?
                        EF.Functions.Like(e.Customer.Name, key) : EF.Functions.Like(e.CustomerOther!.ToLower(), key));
                }

                if (model.Status.HasValue)
                {
                    query = query.Where(e => e.Status == model.Status.Value);
                }

                var totalItem = 0;
                if (model.PageSize > 0)
                {
                    query = query.ApplyPaging(model.PageNo, model.PageSize, out totalItem);
                }
                var result = query.ToList();
                return new BasePaginationResponseModel<PurchaseOrderResponseModel>(model.PageNo, model.PageSize, result, totalItem);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                throw;
            }
        }

        public async Task<bool> Update(long id, PurchaseOrderRequestModel model)
        {
            var purchaseOrder = await _context.PurchaseOrder.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
            if (purchaseOrder != null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(id)));

            var customer = await _context.ApplicationUser.FirstOrDefaultAsync(x => !x.IsDeleted && x.IsActive && x.Id == model.CustomerId);

            purchaseOrder!.Customer = customer;
            purchaseOrder.CustomerOther = model.CustomerOther;
            purchaseOrder.AddressCustomerOther = model.AddressCustomerOther;

            _context.PurchaseOrder.Update(purchaseOrder);
            return await _context.SaveChangesAsync() > 0;
        }

        private IQueryable<PurchaseOrderResponseModel> GetAll()
        {
            return _context.PurchaseOrder.Include(x => x.Customer).Where(x => !x.IsDeleted)
                .Select(x => new PurchaseOrderResponseModel
                {
                    Id = x.Id,
                    Customer = x.Customer != null ? new UserMapper
                    {
                        Id = x.Customer.Id,
                        Name = x.Customer.FullName,
                        PhoneNumber = x.Customer.PhoneNumber,
                        Avatar = x.Customer.Avatar,
                        Country = x.Customer.Country,
                        Province = x.Customer.Province,
                        District = x.Customer.District,
                        Wards = x.Customer.Wards,
                        Address = x.Customer.Address
                    } : null,
                    CustomerOther = x.CustomerOther,
                    AddressCustomerOther = x.AddressCustomerOther,
                    TotalPrice = x.TotalPrice,
                    Status = x.Status,
                    NguoiTao = x.NguoiTao,
                    NgayTao = x.NgayTao,
                    NguoiCapNhat = x.NguoiCapNhat,
                    NgayCapNhat = x.NgayCapNhat
                }).AsQueryable();
        }
    }
}
