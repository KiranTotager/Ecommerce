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
    [ApiExplorerSettings(GroupName ="CMS")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService= productService;
        }
        [HttpPost("product/add")]
        [Authorize(Policy = "AdminOrMangerOrHRPolicy")]
        [SwaggerOperation("use this end call to add new procuct")]
        public async Task<ActionResult<ResponseBase<Object>>> AddProductAsync([FromForm] AddProductDto addProductDto)
        {
            await _productService.AddProductAsync(addProductDto);
            return Ok(new ResponseBase<Object>(StatusCodes.Status201Created,"product addedd successffully"));
        }

        [HttpGet("product/get/all")]
        [Authorize(Policy = "AdminOrMangerOrHRPolicy")]
        [SwaggerOperation("use this end call to get all the product details")]
        public async Task<ActionResult<ProductResponseDto>> GetAllProductsAsync()
        {
            return Ok(await _productService.GetAllProductsAsync());
        }
    }
}
