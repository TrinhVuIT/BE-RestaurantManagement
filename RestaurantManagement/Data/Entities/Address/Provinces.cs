using RestaurantManagement.Commons;

namespace RestaurantManagement.Data.Entities.Address
{
    public class Provinces : BaseEntity
    {
        //Mã Tỉnh Thành
        public string ProvinceCode { get; set; }
        public string ProvinceNameEN { get; set; }
        public string ProvinceNameVNI { get; set; }
        public string Level { get; set; }
        public virtual ListOfCountries? Country { get; set; }
        public virtual IEnumerable<Districts> ListDistrict { get; set; }
    }
}
