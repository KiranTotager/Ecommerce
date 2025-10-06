using ECommerce.DTO;
using ECommerce.DTOs.CustomerDTOs;
using ECommerce.Interfaces.IServices.ICustomerServices;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [ApiExplorerSettings(GroupName = "Customer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class ContactUsController : ControllerBase
    {
        private readonly IContactUsService _contactUsService;
        public ContactUsController(IContactUsService contactUsService)
        {
            _contactUsService = contactUsService;
        }
        [HttpPost("contact/us/add")]
        public async Task<ActionResult<ResponseBase<Object>>> AddContactUsAsync([FromBody] ContactUsDto contactUs)
        {
            await _contactUsService.AddContactUsAsync(contactUs);
            return Ok(new ResponseBase<Object>(StatusCodes.Status200OK, "contact us message stored successfully"));
        }
    }
}
