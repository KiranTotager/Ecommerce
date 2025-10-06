using ECommerce.DTO;
using ECommerce.DTOs.CMSDTOs;
using ECommerce.Interfaces.IServices.ICMSServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ECommerce.Controllers
{
    
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiExplorerSettings(GroupName ="CMS")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [Authorize(Policy = "AdminOrMangerOrHRPolicy")]
        [HttpPost("category/add")]
        [SwaggerOperation("use this end call to add the new category to the database")]
        public async Task<ActionResult<ResponseBase<Object>>> AddCategoryAsync([FromBody] AddCategoryDto addCategoryDto)
        {
            await _service.AddCategoryAsync(addCategoryDto);
            return Ok(new ResponseBase<Object>(StatusCodes.Status200OK, "category addedd successfully"));
        }


        [HttpGet("category/get/all")]
        [SwaggerOperation("use this end call to get all availalbe categories details")]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAllCategoriesAsync()
        {
            return Ok(await _service.GetAllCategoriesAsync());
        }
    }
}
