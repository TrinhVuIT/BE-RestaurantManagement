using RestaurantManagement.Commons;

namespace RestaurantManagement.Data.Entities.Address
{
    public class Districts : BaseEntity
    {
        //Mã Quận Huyện
        public string DistrictCode { get; set; }
        public string DistrictNameEN { get; set; }
        public string DistrictNameVNI { get; set;}
        public string Level { get; set; }
        public virtual Provinces? Province { get; set; }
        public virtual IEnumerable<Wards> ListWards { get; set; }
    }
}
