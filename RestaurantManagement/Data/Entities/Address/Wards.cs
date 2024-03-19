using RestaurantManagement.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagement.Data.Entities.Address
{
    public class Wards : BaseEntity
    {
        //Mã Xã phường
        public string WardCode { get; set; }
        public string WardNameEN { get; set; }
        public string WardNameVNI { get; set; }
        public string Level { get; set; }
        public long DistrictId { get; set; }
        [ForeignKey(nameof(DistrictId))]
        public virtual Districts District { get; set; }
        public long ProvinceId { get; set; }
        [ForeignKey(nameof(ProvinceId))]
        public virtual Provinces Province { get; set; }
    }
}
