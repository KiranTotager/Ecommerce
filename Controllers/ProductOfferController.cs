using ECommerce.DTO;
using ECommerce.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Swashbuckle.AspNetCore.Annotations;

namespace ECommerce.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "CMS")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductOfferController : ControllerBase
    {
        private readonly IHubContext<MessageHubClient, IMessageHubClient> _hubContext;
        public ProductOfferController(IHubContext<MessageHubClient,IMessageHubClient> hubContext)
        {
            _hubContext = hubContext;
        }
        [HttpGet("send/offers/all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation("use this end call to send offers to all connected users")]
        public async Task<ActionResult<ResponseBase<Object>>> sendOffersToAllUsers()
        {
            await _hubContext.Clients.All.SendOffers(new List<string> { "50 % off","buy one get one free","deepavali offer","90 % off"});
            return Ok(new ResponseBase<Object>(StatusCodes.Status200OK,"offers sent to all connected users"));
        }

    }
}
