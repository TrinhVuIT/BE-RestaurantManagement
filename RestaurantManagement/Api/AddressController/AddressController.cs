using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Business.AddressService;
using RestaurantManagement.Commons;
using RestaurantManagement.Data.RequestModels;

namespace RestaurantManagement.Api.AddressController
{
    [Route(Constants.AppSettingKeys.DEFAULT_CONTROLLER_ROUTE)]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }
        [HttpGet]
        public async Task<IActionResult> GetListOfCountries()
        {
            var res = await _addressService.GetListOfCountries();
            return Ok(res);
        }
        [HttpPost]
        public async Task<IActionResult> AddNewCountry([FromBody] CountryRequestModel model)
        {
            var res = await _addressService.AddNewCountry(model);
            return Ok(res);
        }
        [HttpGet]
        public async Task<IActionResult> GetListProvinces([FromQuery] long? countryId)
        {
            var res = await _addressService.GetListProvinces(countryId);
            return Ok(res);
        }
        [HttpGet]
        public async Task<IActionResult> GetListDistrictsByProvince([FromQuery]long provinceId)
        {
            var res = await _addressService.GetListDistrictsByProvince(provinceId);
            return Ok(res);
        }
        [HttpGet]
        public async Task<IActionResult> GetListWardsByDistrict([FromQuery]long districtId)
        {
            var res = await _addressService.GetListWardsByDistrict(districtId);
            return Ok(res);
        }
    }
}
