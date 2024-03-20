using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Business.OrderServices.SupplierService;
using RestaurantManagement.Commons;
using RestaurantManagement.Data.RequestModels.Order;

namespace RestaurantManagement.Api.Controllers.OrderController
{
    [Route(Constants.AppSettingKeys.DEFAULT_CONTROLLER_ROUTE)]
    [ApiController]
    [Authorize]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }
        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] GetPagedSupplierRequestModel model)
        {
            var res = await _supplierService.GetPaged(model);
            return Ok(res);
        }
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] long id)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _supplierService.GetById(id);
            return Ok(res);
        }
        [HttpPost]
        public async Task<IActionResult> CreateNew([FromBody] SupplierRequestModel model)
        {
            var res = await _supplierService.CreateNew(model);
            if (!res)
                return Problem(detail: "Addition unsuccessful", statusCode: 500);
            return Ok(res);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] long id, [FromBody] SupplierRequestModel model)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _supplierService.Update(id, model);
            if (!res)
                return Problem(detail: "Update unsuccessful", statusCode: 500);
            return Ok(res);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] long id)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _supplierService.Delete(id);
            if (!res)
                return Problem(detail: "Delete unsuccessful", statusCode: 500);
            return Ok(res);
        }
    }
}
