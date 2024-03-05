using RestaurantManagement.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagement.Data.Entities.Address
{
    public class Provinces : BaseEntity
    {
        public string ProvinceCode { get; set; }
        public string ProvinceNameEN { get; set; }
        public string ProvinceNameVNI { get; set; }
        public string Level { get; set; }
        public long CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public virtual ListOfCountries Country { get; set; }
        public virtual IEnumerable<Districts> ListDistrict { get; set; }
    }
}
