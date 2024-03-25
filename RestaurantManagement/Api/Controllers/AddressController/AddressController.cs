using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Business.AddressService;
using RestaurantManagement.Commons;
using RestaurantManagement.Data.RequestModels;
using static RestaurantManagement.Commons.Enums;

namespace RestaurantManagement.Api.Controllers.AddressController
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
        public async Task<IActionResult> GetListDistrictsByProvince([FromQuery] long provinceId)
        {
            var res = await _addressService.GetListDistrictsByProvince(provinceId);
            return Ok(res);
        }
        [HttpGet]
        public async Task<IActionResult> GetListWardsByDistrict([FromQuery] long districtId)
        {
            var res = await _addressService.GetListWardsByDistrict(districtId);
            return Ok(res);
        }

        /// <summary>
        /// SeedData ProvincesDistrictsWard
        /// </summary>
        /// <param name="classification">1 = Provinces, 2 = Districts, 3 = Wards</param>
        /// <param name="file">File Excel</param>
        /// <returns>Save Result dbo.Provinces, dbo.Districts, dbo.Ward</returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadProvincesDistrictsWardFromFileExcel(Classification classification, IFormFile file)
        {
            if (classification == 0 || file == null)
                return Ok(false);
            try
            {
                var res = await _addressService.UploadProvincesDistrictsWardFromFileExcel(classification, file);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
