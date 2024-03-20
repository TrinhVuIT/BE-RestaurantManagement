using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Business.OrderServices.StockOutService;
using RestaurantManagement.Commons;
using RestaurantManagement.Data.RequestModels.Order;

namespace RestaurantManagement.Api.Controllers.OrderController
{
    [Route(Constants.AppSettingKeys.DEFAULT_CONTROLLER_ROUTE)]
    [ApiController]
    [Authorize]
    public class StockOutController : ControllerBase
    {
        private readonly IStockOutService _stockOutService;
        public StockOutController(IStockOutService stockOutService)
        {
            _stockOutService = stockOutService;
        }
        [HttpGet]
        public async Task<IActionResult> Getpaged([FromQuery] GetPagedStockOutRequestModel model)
        {
            var res = await _stockOutService.GetPaged(model);
            return Ok(res);
        }
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] long id)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _stockOutService.GetById(id);
            return Ok(res);
        }
        [HttpPost]
        public async Task<IActionResult> CreateNew([FromBody] StockOutRequestModel model)
        {
            var res = await _stockOutService.CreateNew(model);
            if (!res)
                return Problem(detail: "Addition unsuccessful", statusCode: 500);
            return Ok(res);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] long id, [FromBody] StockOutRequestModel model)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _stockOutService.Update(id, model);
            if (!res)
                return Problem(detail: "Update unsuccessful", statusCode: 500);
            return Ok(res);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] long id)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _stockOutService.Delete(id);
            if (!res)
                return Problem(detail: "Delete unsuccessful", statusCode: 500);
            return Ok(res);
        }
    }
}
