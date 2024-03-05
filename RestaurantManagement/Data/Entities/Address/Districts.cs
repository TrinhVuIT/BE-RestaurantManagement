using RestaurantManagement.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagement.Data.Entities.Address
{
    public class Districts : BaseEntity
    {
        public string DistrictCode { get; set; }
        public string DistrictNameEN { get; set; }
        public string DistrictNameVNI { get; set;}
        public string Level { get; set; }
        public long? ProvinceId { get; set; }
        [ForeignKey(nameof(ProvinceId))]
        public virtual Provinces Province { get; set; }
        public virtual IEnumerable<Wards> ListWards { get; set; }
    }
}
