using ECommerce.DTO;
using ECommerce.DTOs.CustomerDTOs;
using ECommerce.Interfaces.IServices.ICustomerServices;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ECommerce.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName ="Customer")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class CartController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;
        public CartController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }
        [HttpPost("cart/add")]
        [SwaggerOperation("use this end call to add the product to cart list")]
        public async Task<ActionResult<ResponseBase<Object>>> AddToCartAsync([FromBody] AddCartDTO addCartDTO)
        {
            await _cartItemService.AddToCartAsync(addCartDTO);
            return Ok(new ResponseBase<Object>(StatusCodes.Status201Created, "item added to cart successfully"));
        }
        [HttpGet("cart/items/get")]
        [SwaggerOperation("use this end call to get all the cart itmes of belongs to customer/guest")]
        public async Task<ActionResult<IEnumerable<CartItemResponseDto>>> GetCartItemsAsync()
        {
            return Ok(await _cartItemService.GetAllCartItemsAsync());
        }
    }
}
