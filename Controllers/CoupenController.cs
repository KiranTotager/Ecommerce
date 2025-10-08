using ECommerce.DTO;
using ECommerce.DTOs.CMSDTOs;
using ECommerce.DTOs.CustomerDTOs;
using ECommerce.Interfaces.IServices.ICMSServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ECommerce.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
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
        [Authorize(Policy = "AdminOrMangerOrHRPolicy")]
        [ApiExplorerSettings(GroupName = "CMS")]
        [SwaggerOperation("hit this end call to add the new coupen")]
        public async Task<ActionResult<ResponseBase<Object>>> AddCoupeAsync([FromBody] AddCoupenDetailsDTO addCoupenDetailsDTO)
        {
            await _coupenService.AddCoupenAsync(addCoupenDetailsDTO);
            return Ok(new ResponseBase<Object>(StatusCodes.Status200OK, "coupen addedd successfully"));
        }

        [HttpGet("get/cms/coupens")]
        [Authorize(Policy = "AdminOrMangerOrHRPolicy")]
        [ApiExplorerSettings(GroupName ="CMS")]
        [SwaggerOperation("hit this end call to get the all available coupens")]
        public async Task<ActionResult<IEnumerable<CMSCoupenResponseDto>>> CMSGetAllCoupensAsync()
        {
            return Ok(await _coupenService.CMSGetAllCoupensAsync());
        }

        [HttpGet("get/customer/coupens")]
        [ApiExplorerSettings(GroupName ="Customer")]
        [SwaggerOperation("hit this end call to get the available active coupens")]
        public async Task<ActionResult<IEnumerable<CustomerCoupenDetailsResponseDto>>> CustomerGetAllCoupensAsync()
        {
            return Ok(await _coupenService.CustomerGetAllACtiveCoupensAsync());
        }
    }
}
