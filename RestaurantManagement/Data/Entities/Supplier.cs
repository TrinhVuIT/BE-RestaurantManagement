using RestaurantManagement.Commons;

namespace RestaurantManagement.Data.Entities
{
    //Nhà cung cấp
    public class Supplier : BaseEntityCommons
    {
        public string SupplierName { get; set; }
        public string? SupplierAddress { get; set;}
        public string? PhoneNumber { get; set;}
    }
}
