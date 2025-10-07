using ECommerce.DTO;
using ECommerce.DTOs.CMSDTOs;
using ECommerce.DTOs.CustomerDTOs;
using ECommerce.Interfaces.IServices.ICMSServices;
using ECommerce.Interfaces.IServices.ICustomerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ECommerce.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICustomerProductService _customerProductService;
        public ProductController(IProductService productService,ICustomerProductService customerProductService)
        {
            _productService= productService;
            _customerProductService= customerProductService;
        }
        [HttpPost("product/add")]
        [ApiExplorerSettings(GroupName = "CMS")]
        [Authorize(Policy = "AdminOrMangerOrHRPolicy")]
        [SwaggerOperation("use this end call to add new procuct")]
        public async Task<ActionResult<ResponseBase<Object>>> AddProductAsync([FromForm] AddProductDto addProductDto)
        {
            await _productService.AddProductAsync(addProductDto);
            return Ok(new ResponseBase<Object>(StatusCodes.Status201Created,"product addedd successffully"));
        }

        [HttpGet("product/get/all")]
        [ApiExplorerSettings(GroupName = "CMS")]
        [Authorize(Policy = "AdminOrMangerOrHRPolicy")]
        [SwaggerOperation("use this end call to get all the product details")]
        public async Task<ActionResult<ProductResponseDto>> GetAllProductsAsync()
        {
            return Ok(await _productService.GetAllProductsAsync());
        }

        [HttpGet("customer/product/get/all")]
        [ApiExplorerSettings(GroupName ="Customer")]
        [SwaggerOperation("use this end call for displaying the products to customer")]
        public async Task<ActionResult<CustomerProductResponseDto>> GetAllProductsForCustomerAsync()
        {
            return Ok(await _customerProductService.GetAllProductsAsnc());
        }
    }
}
