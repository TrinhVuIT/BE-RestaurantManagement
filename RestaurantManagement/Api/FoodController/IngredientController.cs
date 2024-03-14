using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Business.FoodServices.IngredientService;
using RestaurantManagement.Commons;
using RestaurantManagement.Data.RequestModels.Food;

namespace RestaurantManagement.Api.FoodController
{
    [Route(Constants.AppSettingKeys.DEFAULT_CONTROLLER_ROUTE)]
    [ApiController]
    [Authorize]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;
        public IngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }
        [HttpGet]
        public async Task<IActionResult> GetPaged(GetPagedIngredientRequestModel model)
        {
            var res = await _ingredientService.GetPaged(model);
            return Ok(res);
        }
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] long id)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _ingredientService.GetById(id);
            return Ok(res);
        }
        [HttpPost]
        public async Task<IActionResult> CreateNew([FromBody] IngredientRequestModel model)
        {
            var res = await _ingredientService.CreateNew(model);
            if (!res)
                return Problem(detail: "Addition unsuccessful", statusCode: 500);
            return Ok(res);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] long id, [FromBody]IngredientRequestModel model)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _ingredientService.Update(id, model);
            if (!res)
                return Problem(detail: "Update unsuccessful", statusCode: 500);
            return Ok(res);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] long id)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _ingredientService.Delete(id);
            if (!res)
                return Problem(detail: "Update unsuccessful", statusCode: 500);
            return Ok(res);
        }
    }
}
