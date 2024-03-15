using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Business.FoodServices.IngredientDetailService;
using RestaurantManagement.Commons;
using RestaurantManagement.Data.RequestModels.Food;

namespace RestaurantManagement.Api.FoodController
{
    [Route(Constants.AppSettingKeys.DEFAULT_CONTROLLER_ROUTE)]
    [ApiController]
    [Authorize]
    public class IngredientDetailController : ControllerBase
    {
        private readonly IIngredientDetailService _ingredientDetailService;
        public IngredientDetailController(IIngredientDetailService ingredientDetailService)
        {
            _ingredientDetailService = ingredientDetailService;
        }
        [HttpGet]
        public async Task<IActionResult> GetPagedByRecipeId([FromQuery] GetPagedIngredientDetailRequestModel model)
        {
            var res = await _ingredientDetailService.GetPagedByRecipeId(model);
            return Ok(res);
        }
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] long id)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);

            var res = await _ingredientDetailService.GetById(id);
            return Ok(res);
        }
        [HttpPost]
        public async Task<IActionResult> CreateNew([FromBody] IngredientDetailRequestModel model)
        {
            var res = await _ingredientDetailService.CreateNew(model);
            if (!res)
                return Problem(detail: "Addition unsuccessful", statusCode: 500);
            return Ok(res);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] long id, [FromBody] UpdateIngredientDetailRequestModel model)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _ingredientDetailService.Update(id, model);
            if(!res)
                return Problem(detail: "Update unsuccessful", statusCode: 500);
            return Ok(res);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] long id)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _ingredientDetailService.Delete(id);
            if (!res)
                return Problem(detail: "Delete unsuccessful", statusCode: 500);
            return Ok(res);
        }
    }
}
