using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Business.PurchaseOrderService.PurchaseOrderFoodService;
using RestaurantManagement.Commons;
using RestaurantManagement.Data.RequestModels.PurchaseOrder;

namespace RestaurantManagement.Api.Controllers.PurchaseOrderController
{
    [Route(Constants.AppSettingKeys.DEFAULT_CONTROLLER_ROUTE)]
    [ApiController]
    [Authorize]
    public class PurchaseOrderFoodController : ControllerBase
    {
        private readonly IPurchaseOrderFoodService _purchaseOrderFoodService;
        public PurchaseOrderFoodController(IPurchaseOrderFoodService purchaseOrderFoodService)
        {
            _purchaseOrderFoodService = purchaseOrderFoodService;
        }
        [HttpGet]
        public async Task<IActionResult> GetPagedByPurchaseOrderId([FromQuery] GetPagedPurchaseOrderFoodRequestModel model)
        {
            var res = await _purchaseOrderFoodService.GetPagedByPurchaseOrderId(model);
            return Ok(res);
        }
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] long id)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _purchaseOrderFoodService.GetById(id);
            return Ok(res);
        }
        [HttpPost]
        public async Task<IActionResult> CreateNew([FromBody] PurchaseOrderFoodRequestModel model)
        {
            var res = await _purchaseOrderFoodService.CreateNew(model);
            if (!res)
                return Problem(detail: "Addition unsuccessful", statusCode: 500);
            return Ok(res);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] long id, [FromBody] UpdatePurchaseOrderFoodRequestModel model)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _purchaseOrderFoodService.Update(id, model);
            if (!res)
                return Problem(detail: "Update unsuccessful", statusCode: 500);
            return Ok(res);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] long id)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _purchaseOrderFoodService.Delete(id);
            if (!res)
                return Problem(detail: "Delete unsuccessful", statusCode: 500);
            return Ok(res);
        }
    }
}
