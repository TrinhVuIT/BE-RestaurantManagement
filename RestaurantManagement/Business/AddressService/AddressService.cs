using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities.Address;
using RestaurantManagement.Data.RequestModels;

namespace RestaurantManagement.Business.AddressService
{
    public class AddressService : IAddressService
    {
        private readonly DataContext _context;
        public AddressService(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> AddNewCountry(CountryRequestModel countryRequest)
        {
            if(countryRequest == null) return false;

            var country = new ListOfCountries()
            {
                CountryCode = countryRequest.CountryCode,
                CountryNameEN = countryRequest.CountryNameEN,
                CountryNameVNI = countryRequest.CountryNameVNI,
            };
            _context.Countries.Add(country);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<ListOfCountries>> GetListOfCountries()
        {
            return await _context.Countries.ToListAsync();
        }
        public async Task<List<Districts>> GetListDistrictsByProvince(long provinceId)
        {
            var province = await _context.Provinces.Include(x => x.ListDistrict).FirstOrDefaultAsync(x => x.Id == provinceId);
            if(province == null)
                return new List<Districts>();

            return province.ListDistrict.Select(x => new Districts
            {
                Id = x.Id,
                DistrictCode = x.DistrictCode,
                DistrictNameEN = x.DistrictNameEN,
                DistrictNameVNI = x.DistrictNameVNI,
            }).ToList();
        }

        public async Task<List<Provinces>> GetListProvinces(long? countryId)
        {
            if(countryId != null)
                return await _context.Provinces.Include(x => x.Country).Where(x => x.Country.Id == countryId).ToListAsync();
            return await _context.Provinces.ToListAsync();
        }

        public async Task<List<Wards>> GetListWardsByDistrict(long districtId)
        {
            var district = await _context.Districts.Include(x => x.ListWards).FirstOrDefaultAsync(x => x.Id == districtId);
            if(district == null)
                return new List<Wards>();

            return district.ListWards.Select(x => new Wards
            {
                Id = x.Id,
                WardCode = x.WardCode,
                WardNameEN = x.WardNameEN,
                WardNameVNI = x.WardNameVNI,
            }).ToList();
        }
    }
}
