using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Business.EmailCofigServices;
using RestaurantManagement.Commons;
using RestaurantManagement.Data.RequestModels.Email;

namespace RestaurantManagement.Api
{
    [Route(Constants.AppSettingKeys.DEFAULT_CONTROLLER_ROUTE)]
    [ApiController]
    [Authorize]
    public class EmailConfigController : ControllerBase
    {
        public readonly IEmailConfigServices _emailConfigServices;

        public EmailConfigController(IEmailConfigServices emailConfigServices)
        {
            _emailConfigServices = emailConfigServices;
        }
        [HttpPost]
        public async Task<ActionResult> AddNewEmailConfig([FromBody] EmailConfigRequestModel newEmail)
        {
            var result = await _emailConfigServices.AddNewEmailConfig(newEmail);
            return Ok(result);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateEmailConfig([FromQuery] long id, [FromBody] EmailConfigRequestModel updateEmail)
        {
            var result = await _emailConfigServices.UpdateEmailConfig(id, updateEmail);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteEmail([FromQuery] long id)
        {
            var result = await _emailConfigServices.DeleteEmail(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> SendToEmailAsync([FromBody] SendToEmailRequestModel emailModel)
        {
            var result = await _emailConfigServices.SendToEmailAsync(emailModel);
            return Ok(result);
        }
    }
}
