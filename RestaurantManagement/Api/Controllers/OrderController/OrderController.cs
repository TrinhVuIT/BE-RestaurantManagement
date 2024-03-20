using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Business.OrderServices;
using RestaurantManagement.Commons;
using RestaurantManagement.Data.RequestModels.Order;

namespace RestaurantManagement.Api.Controllers.OrderController
{
    [Route(Constants.AppSettingKeys.DEFAULT_CONTROLLER_ROUTE)]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] GetPagedOrderRequestModel model)
        {
            var res = await _orderService.GetPaged(model);
            return Ok(res);
        }
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] long id)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _orderService.GetById(id);
            return Ok(res);
        }
        [HttpPost]
        public async Task<IActionResult> CreateNew([FromBody] OrderRequestModel model)
        {
            var res = await _orderService.CreateNew(model);
            if (!res)
                return Problem(detail: "Addition unsuccessful", statusCode: 500);
            return Ok(res);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] long id, [FromBody] OrderRequestModel model)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _orderService.Update(id, model);
            if (!res)
                return Problem(detail: "Update unsuccessful", statusCode: 500);
            return Ok(res);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] long id)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _orderService.Delete(id);
            if (!res)
                return Problem(detail: "Delete unsuccessful", statusCode: 500);
            return Ok(res);
        }
    }
}
