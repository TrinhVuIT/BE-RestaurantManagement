using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Commons;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities;
using RestaurantManagement.Data.RequestModels.Order;
using RestaurantManagement.Data.ResponseModels;
using RestaurantManagement.Data.ResponseModels.FoodResponseModel;
using RestaurantManagement.Data.ResponseModels.Order;
using static RestaurantManagement.Commons.Constants;

namespace RestaurantManagement.Business.OrderServices.OrderDetailService
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly DataContext _context;
        public OrderDetailService(DataContext context) {  _context = context; }
        public async Task<bool> CreateNew(OrderDetailRequestModel model)
        {
            var order = await _context.Order.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == model.OrderId);
            if (order == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(model.OrderId)));

            var ingredient = await _context.Ingredient.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == model.IngredientId);
            if (ingredient == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(model.IngredientId)));

            var newOrderDetail = new OrderDetail()
            {
                Order = order,
                Ingredient = ingredient,
                Quantity = model.Quantity,
            };
            _context.OrderDetail.Add(newOrderDetail);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(long id)
        {
            var orderDetail = await _context.OrderDetail.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
            if (orderDetail == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(id)));
            orderDetail.IsDeleted = true;
            _context.OrderDetail.Update(orderDetail);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<OrderDetailReponseModel?> GetById(long id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BasePaginationResponseModel<OrderDetailReponseModel>> GetPagedByOrderId(GetPagedOrderDetailRequestModel model)
        {
            try
            {
                var query = GetAll().Where(x => x.Order.Id == model.OrderId).OrderByDescending(x => x.NgayCapNhat).AsQueryable();

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
                List<OrderDetailReponseModel> result = query.ToList();
                return new BasePaginationResponseModel<OrderDetailReponseModel>(model.PageNo, model.PageSize, result, totalItem);

            }catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                throw;
            }
        }

        public async Task<bool> Update(long id, UpdateOrderDetailRequestModel model)
        {
            var orderDetail = await _context.OrderDetail.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
            if (orderDetail == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(id)));

            var ingredient = await _context.Ingredient.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == model.IngredientId);
            if (ingredient == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(model.IngredientId)));

            orderDetail.Ingredient = ingredient;
            orderDetail.Quantity = model.Quantity;

            _context.OrderDetail.Update(orderDetail);
            return await _context.SaveChangesAsync() > 0;
        }
        private IQueryable<OrderDetailReponseModel> GetAll()
        {
            return _context.OrderDetail.Include(x => x.Order).ThenInclude(o => o.Supplier).Include(x => x.Ingredient)
                .Where(x => !x.IsDeleted).Select(x => new OrderDetailReponseModel
                {
                    Id = x.Id,
                    Order = new OrderMapper
                    {
                        Id = x.Order.Id,
                        SupplierId = x.Order.Supplier.Id,
                        SupplierName = x.Order.Supplier.SupplierName,
                        SupplierAddress = x.Order.Supplier.SupplierAddress,
                        SupplierPhoneNumber = x.Order.Supplier.PhoneNumber,
                    },
                    Ingredient = new IngredientMapper
                    {
                        Id = x.Ingredient.Id,
                        IngredientName = x.Ingredient.IngredientName,
                        Exp = x.Ingredient.Exp,
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
