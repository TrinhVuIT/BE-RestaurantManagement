﻿using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities;
using RestaurantManagement.Data.RequestModels.Order;
using RestaurantManagement.Data.ResponseModels;
using RestaurantManagement.Data.ResponseModels.Order;
using static RestaurantManagement.Commons.Constants;
using static RestaurantManagement.Commons.Enums;

namespace RestaurantManagement.Business.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;
        public OrderService(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateNew(OrderRequestModel model)
        {
            var supplier = await _context.Supplier.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == model.SupplierId);
            if (supplier == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(model.SupplierId)));

            var res = new Order()
            {
                Supplier = supplier,
                DeliveryAppointment = model.DeliveryAppointment,
                StatusOrder = StatusOrder.Pending
            };
            _context.Order.Add(res);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(long id)
        {
            var res = await _context.Order.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id ==  id);
            if (res == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(id)));

            var orderDetail = await _context.OrderDetail.Include(x => x.Order).Where(x => !x.IsDeleted && x.Order.Id == id).ToListAsync();
            orderDetail.ForEach(x => x.IsDeleted = true);

            res.IsDeleted = true;

            _context.OrderDetail.UpdateRange(orderDetail);
            _context.Order.Update(res);

            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<OrderResponseModel?> GetById(long id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<BasePaginationResponseModel<OrderResponseModel>> GetPaged(GetPagedOrderRequestModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(long id, OrderRequestModel model)
        {
            var updateOrder = await _context.Order.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
            if(updateOrder == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(id)));

            var supplier = await _context.Supplier.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == model.SupplierId);
            if (supplier == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(model.SupplierId)));

            updateOrder.Supplier = supplier;
            updateOrder.DeliveryAppointment = model.DeliveryAppointment;

            _context.Order.Update(updateOrder);
            return await _context.SaveChangesAsync() > 0;
        }

        private IQueryable<OrderResponseModel> GetAll()
        {
            return _context.Order.Include(x => x.Supplier)
                .Where(x => !x.IsDeleted)
                .Select(x => new OrderResponseModel
                {
                    Id = x.Id,
                    Supplier = new SupplierMapper
                    {
                        Id = x.Supplier.Id,
                        SupplierName = x.Supplier.SupplierName,
                        SupplierAddress = x.Supplier.SupplierAddress,
                        PhoneNumber = x.Supplier.PhoneNumber,
                    },
                    DeliveryAppointment = x.DeliveryAppointment,
                    StatusOrder = x.StatusOrder,
                    NgayTao = x.NgayTao,
                    NgayCapNhat = x.NgayCapNhat,
                    NguoiTao = x.NguoiTao,
                    NguoiCapNhat = x.NguoiCapNhat,
                }).AsQueryable();
        }
    }
}
