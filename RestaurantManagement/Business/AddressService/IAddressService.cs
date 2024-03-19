using RestaurantManagement.Data.Entities.Address;
using RestaurantManagement.Data.RequestModels;

namespace RestaurantManagement.Business.AddressService
{
    public interface IAddressService
    {
        Task<List<ListOfCountries>> GetListOfCountries();
        Task<bool> AddNewCountry(CountryRequestModel country);
        Task<List<Provinces>> GetListProvinces(long? countryId);
        Task<List<Districts>> GetListDistrictsByProvince(long provinceId);
        Task<List<Wards>> GetListWardsByDistrict(long districtId);
    }
}
