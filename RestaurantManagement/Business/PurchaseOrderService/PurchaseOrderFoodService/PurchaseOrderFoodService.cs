using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Commons;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities;
using RestaurantManagement.Data.RequestModels.PurchaseOrder;
using RestaurantManagement.Data.ResponseModels;
using RestaurantManagement.Data.ResponseModels.FoodResponseModel;
using RestaurantManagement.Data.ResponseModels.PurchaseOrder;
using static RestaurantManagement.Commons.Constants;

namespace RestaurantManagement.Business.PurchaseOrderService.PurchaseOrderFoodService
{
    public class PurchaseOrderFoodService : IPurchaseOrderFoodService
    {
        private readonly DataContext _context;
        public PurchaseOrderFoodService(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateNew(PurchaseOrderFoodRequestModel model)
        {
            var purchaseOrder = await _context.PurchaseOrder.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == model.PurchaseOrderId);
            if (purchaseOrder == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(model.PurchaseOrderId)));

            var food = await _context.Food.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == model.FoodId);
            if (food == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(model.FoodId)));

            var newPurchaseOrderFood = new PurchaseOrderFood
            {
                PurchaseOrder = purchaseOrder,
                Food = food,
                Quantity = model.Quantity,
            };

            _context.PurchaseOrderFood.Add(newPurchaseOrderFood);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(long id)
        {
            var purchaseOrderFood = await _context.PurchaseOrderFood.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
            if (purchaseOrderFood == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(id)));

            purchaseOrderFood.IsDeleted = true;

            _context.PurchaseOrderFood.Update(purchaseOrderFood);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<PurchaseOrderFoodResponseModel?> GetById(long id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BasePaginationResponseModel<PurchaseOrderFoodResponseModel>> GetPagedByPurchaseOrderId(GetPagedPurchaseOrderFoodRequestModel model)
        {
            try
            {
                var query = GetAll().Where(x => x.PurchaseOrder.Id == model.PurchaseOrderId).OrderByDescending(x => x.NgayCapNhat).AsQueryable();

                if (!string.IsNullOrEmpty(model.Keyword))
                {
                    var key = model.Keyword.ToLower().Trim();
                    query = query.Where(e => EF.Functions.Like(e.Food.FoodName, key));
                }

                var totalItem = 0;
                if(model.PageSize > 0)
                {
                    query = query.ApplyPaging(model.PageNo,model.PageSize, out totalItem);
                }
                var result = query.ToList();
                return new BasePaginationResponseModel<PurchaseOrderFoodResponseModel>(model.PageNo, model.PageSize, result, totalItem);

            }catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                throw;
            }
        }

        public async Task<bool> Update(long id, UpdatePurchaseOrderFoodRequestModel model)
        {
            var purchaseOrderFood = await _context.PurchaseOrderFood.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
            if (purchaseOrderFood == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(id)));

            var food = await _context.Food.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == model.FoodId);
            if (food == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(model.FoodId)));

            purchaseOrderFood.Food = food;
            purchaseOrderFood.Quantity = model.Quantity;

            _context.PurchaseOrderFood.Update(purchaseOrderFood);
            return await _context.SaveChangesAsync() > 0;
        }

        private IQueryable<PurchaseOrderFoodResponseModel> GetAll()
        {
            return _context.PurchaseOrderFood.Include(x => x.Food).Include(x => x.PurchaseOrder).ThenInclude(x => x.Customer)
                .Where(x => !x.IsDeleted)
                .Select(x => new PurchaseOrderFoodResponseModel
                {
                    Id = x.Id,
                    PurchaseOrder = new PurchaseOrderMapper
                    {
                        Id = x.PurchaseOrder.Id,
                        Customer = x.PurchaseOrder.Customer != null ?
                        new UserMapper
                        {
                            Id = x.PurchaseOrder.Customer.Id,
                            Name = x.PurchaseOrder.Customer.FullName,
                            PhoneNumber = x.PurchaseOrder.Customer.PhoneNumber,
                            Avatar = x.PurchaseOrder.Customer.Avatar,
                            Country = x.PurchaseOrder.Customer.Country,
                            Province = x.PurchaseOrder.Customer.Province,
                            District = x.PurchaseOrder.Customer.District,
                            Wards = x.PurchaseOrder.Customer.Wards,
                            Address = x.PurchaseOrder.Customer.Address
                        } : null,
                        CustomerOther = x.PurchaseOrder.CustomerOther,
                        AddressCustomerOther = x.PurchaseOrder.CustomerOther,
                        TotalPrice = x.PurchaseOrder.TotalPrice,
                        Status = x.PurchaseOrder.Status
                    },
                    Food = new FoodMapper
                    {
                        Id = x.Food.Id,
                        FoodName = x.Food.FoodName,
                        FoodDescription = x.Food.FoodDescription,
                        FoodPrice = x.Food.FoodPrice,
                    },
                    Quantity = x.Quantity,
                    NgayTao = x.NgayTao,
                    NgayCapNhat = x.NgayCapNhat,
                    NguoiTao = x.NguoiTao,
                    NguoiCapNhat = x.NguoiCapNhat,
                }).AsQueryable();
        }
    }
}
