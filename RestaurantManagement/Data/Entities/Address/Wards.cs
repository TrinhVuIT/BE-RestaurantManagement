using RestaurantManagement.Commons;

namespace RestaurantManagement.Data.Entities.Address
{
    public class Wards : BaseEntity
    {
        //Mã Xã phường
        public string WardCode { get; set; }
        public string WardNameEN { get; set; }
        public string WardNameVNI { get; set; }
        public string Level { get; set; }
        public virtual Districts? District { get; set; }
        public virtual Provinces? Province { get; set; }
    }
}
