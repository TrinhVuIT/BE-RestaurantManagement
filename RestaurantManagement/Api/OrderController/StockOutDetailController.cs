using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Business.OrderServices.StockOutDetailService;
using RestaurantManagement.Data.RequestModels.Order;

namespace RestaurantManagement.Api.OrderController
{
    public class StockOutDetailController : ControllerBase
    {
        private readonly IStockOutDetailService _stockOutDetailService;
        public StockOutDetailController(IStockOutDetailService stockOutDetailService)
        {
            _stockOutDetailService = stockOutDetailService;
        }
        [HttpGet]
        public async Task<IActionResult> GetPagedByStockOutId([FromQuery] GetPagedStockOutDetailRequestModel model)
        {
            var res = await _stockOutDetailService.GetPagedByStockoutId(model);
            return Ok(res);
        }
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery]long id)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _stockOutDetailService.GetById(id);
            return Ok(res);
        }
        [HttpPost]
        public async Task<IActionResult> CreateNew([FromBody] StockOutDetailRequestModel model)
        {
            var res = await _stockOutDetailService.CreateNew(model);
            if (!res)
                return Problem(detail: "Addition unsuccessful", statusCode: 400);
            return Ok(res);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] long id, [FromBody] UpdateStockOutDetailRequestModel model)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _stockOutDetailService.Update(id, model);
            if (!res)
                return Problem(detail: "Update unsuccessful", statusCode: 500);
            return Ok(res);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] long id)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _stockOutDetailService.Delete(id);
            if(!res)
                return Problem(detail:"Delete unsuccessful",statusCode: 500);
            return Ok(res);
        }
    }
}
