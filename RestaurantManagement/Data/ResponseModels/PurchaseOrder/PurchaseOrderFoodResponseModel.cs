using RestaurantManagement.Data.ResponseModels.FoodResponseModel;
using static RestaurantManagement.Commons.Enums;

namespace RestaurantManagement.Data.ResponseModels.PurchaseOrder
{
    public class PurchaseOrderFoodResponseModel
    {
        public long Id { get; set; }
        public PurchaseOrderMapper PurchaseOrder { get; set; }
        public FoodMapper Food { get; set; }
        public int Quantity { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public string? NguoiTao { get; set; }
        public string? NguoiCapNhat { get; set; }
    }

    public class PurchaseOrderMapper
    {
        public long Id { get; set; }
        public UserMapper? Customer { get; set; }
        public string? CustomerOther { get; set; }
        public string? AddressCustomerOther { get; set; }
        public decimal TotalPrice { get; set; }
        public StatusOrder Status { get; set; }
    }
}
