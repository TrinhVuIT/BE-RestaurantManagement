using RestaurantManagement.Commons;
using System.Collections.Generic;
using static RestaurantManagement.Commons.Enums;

namespace RestaurantManagement.Data.Entities
{
    public class User : BaseEntityCommons
    {
        public string? UserName { get; set; }
        public string? Code { get; set; }
        public Sex? Sex { get; set; }
        //Quốc gia
        public ListOfCountries? Nation { get; set; }
        //Tỉnh thành
        public Provinces? Province { get; set; }
        //Quận huyện
        public Districts? District { get; set; }
        //Xã phường
        public Wards? Wards { get; set; }
    }
}
