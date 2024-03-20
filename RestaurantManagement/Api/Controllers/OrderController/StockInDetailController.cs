using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Business.OrderServices.StockInDetailService;
using RestaurantManagement.Commons;
using RestaurantManagement.Data.RequestModels.Order;

namespace RestaurantManagement.Api.Controllers.OrderController
{
    [Route(Constants.AppSettingKeys.DEFAULT_CONTROLLER_ROUTE)]
    [ApiController]
    [Authorize]
    public class StockInDetailController : ControllerBase
    {
        private readonly IStockInDetailService _stockInDetailService;
        public StockInDetailController(IStockInDetailService stockInDetailService)
        {
            _stockInDetailService = stockInDetailService;
        }
        [HttpGet]
        public async Task<IActionResult> GetPagedByStockInId([FromQuery] GetPagedStockInDetailRequestModel model)
        {
            var res = await _stockInDetailService.GetPagedByStockInId(model);
            return Ok(res);
        }
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] long id)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _stockInDetailService.GetById(id);
            return Ok(res);
        }
        [HttpPost]
        public async Task<IActionResult> CreateNew([FromBody] StockInDetailRequestModel model)
        {
            var res = await _stockInDetailService.CreateNew(model);
            if (!res)
                return Problem(detail: "Addition unsuccessful", statusCode: 500);
            return Ok(res);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] long id, [FromBody] UpdateStockInDetailRequestModel model)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _stockInDetailService.Update(id, model);
            if (!res)
                return Problem(detail: "Update unsuccessful", statusCode: 500);
            return Ok(res);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] long id)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _stockInDetailService.Delete(id);
            if (!res)
                return Problem(detail: "Delete unsuccessful", statusCode: 500);
            return Ok(res);
        }
    }
}
