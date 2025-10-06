using ECommerce.DTO;
using ECommerce.DTOs.CMSDTOs;
using ECommerce.Interfaces.IServices.ICMSServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ECommerce.Controllers
{
    [Authorize(Policy = "AdminOrMangerOrHRPolicy")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiExplorerSettings(GroupName ="CMS")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public class CoupenController : ControllerBase
    {
        private readonly ICoupenService _coupenService;
        public CoupenController(ICoupenService coupenService)
        {
            _coupenService = coupenService;
        }
        [HttpPost("add/coupen")]
        [SwaggerOperation("hit this end call to add the new coupen")]
        public async Task<ActionResult<ResponseBase<Object>>> AddCoupeAsync([FromBody] AddCoupenDetailsDTO addCoupenDetailsDTO)
        {
            await _coupenService.AddCoupenAsync(addCoupenDetailsDTO);
            return Ok(new ResponseBase<Object>(StatusCodes.Status200OK, "coupen addedd successfully"));
        }
    }
}
