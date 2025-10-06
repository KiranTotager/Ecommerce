using ECommerce.DTO;
using ECommerce.DTOs.CustomerDTOs;
using ECommerce.Interfaces.IRepository.IUserRepos;
using ECommerce.Interfaces.IServices.ICustomerServices;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ECommerce.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiExplorerSettings(GroupName ="Customer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService _wishLIstService;
        public WishListController(IWishListService wishLIstService)
        {
            _wishLIstService= wishLIstService;
        }

        [HttpPost("wishlist/add")]
        [SwaggerOperation("use this end call to add item to wishlist")]
        public async Task<ActionResult<ResponseBase<Object>>> AddWishListAsync([FromBody] AddWishListDto addWishListDto)
        {
            await _wishLIstService.AddToWishListAsync(addWishListDto);
            return Ok(new ResponseBase<Object>(StatusCodes.Status200OK,"item addedd to wish list successfully"));
        }

        [HttpGet("wishlist/get/all")]
        [SwaggerOperation("use this end call to get wish list items of guest or customer")]
        public async Task<ActionResult<IEnumerable<WishListResponseDto>>> GetAllWishListItemsAsync()
        {
            return Ok(await _wishLIstService.GetAllWishListsOfUserAsync());
        }
    }
} 
