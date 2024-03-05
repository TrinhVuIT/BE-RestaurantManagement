using RestaurantManagement.Commons;

namespace RestaurantManagement.Data.Entities.Address
{
    public class Wards : BaseEntity
    {
        public string WardCode { get; set; }
        public string WardNameEN { get; set; }
        public string WardNameTV { get; set; }
    }
}
