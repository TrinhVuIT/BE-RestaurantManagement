using RestaurantManagement.Data.ResponseModels.FoodResponseModel;

namespace RestaurantManagement.Data.ResponseModels.Order
{
    public class OrderDetailReponseModel
    {
        public long Id { get; set; }
        public OrderMapper Order {  get; set; }
        public IngredientMapper Ingredient { get; set; }
        public int Quantity { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public string? NguoiTao { get; set; }
        public string? NguoiCapNhat { get; set; }
    }
    public class OrderMapper
    {
        public long Id { get; set; }
            public long SupplierId { get; set; }
            public string SupplierName { get; set; }
            public string? SupplierAddress { get; set; }
            public string? SupplierPhoneNumber { get; set; }
    }
}
