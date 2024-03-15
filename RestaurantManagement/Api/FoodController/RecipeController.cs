using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Business.FoodServices.RecipeService;
using RestaurantManagement.Commons;
using RestaurantManagement.Data.RequestModels.Food;

namespace RestaurantManagement.Api.FoodController
{
    [Route(Constants.AppSettingKeys.DEFAULT_CONTROLLER_ROUTE)]
    [ApiController]
    [Authorize]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;
        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }
        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] GetPagedRecipeRequestModel model)
        {
            var res = await _recipeService.GetPaged(model);
            return Ok(res);
        }
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] long id)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _recipeService.GetById(id);
            return Ok(res);
        }
        [HttpPost]
        public async Task<IActionResult> CreateNew([FromBody] RecipeRequestModel model)
        {
            var res = await _recipeService.CreateNew(model);
            if (!res)
                return Problem(detail: "Addition unsuccessful", statusCode: 500);
            return Ok(res);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] long id, [FromBody] RecipeRequestModel model)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _recipeService.Update(id, model);
            if(!res)
                return Problem(detail: "Update unsuccessful", statusCode: 500);
            return Ok(res);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] long id)
        {
            if (id < 0)
                return Problem(detail: "Invalid ID", statusCode: 400);
            var res = await _recipeService.Delete(id);
            if (!res)
                return Problem(detail: "Delete unsuccessful", statusCode: 500);
            return Ok(res);
        }
    }
}
