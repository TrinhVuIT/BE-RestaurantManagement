using RestaurantManagement.Commons;

namespace RestaurantManagement.Data.Entities.Address
{
    public class ListOfCountries : BaseEntity
    {
        public string? CountryCode { get; set; }
        public string? CountryNameEN { get; set; }
        public string? CountryNameVNI { get; set; }
    }
}
