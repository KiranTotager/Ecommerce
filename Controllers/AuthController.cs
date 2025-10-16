using ECommerce.DTO;
using ECommerce.DTOs.Auth;
using ECommerce.Enums;
using ECommerce.Interfaces.IServices;
using ECommerce.Interfaces.IUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ECommerce.Controllers
{
    /// <summary>
    /// this login controllers
    /// </summary>
    [ApiExplorerSettings(GroupName ="Authentication")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IUserContextService _userContextService;
        public AuthController(IAuthService authService,ILogger<AuthController> logger,IUserContextService userContextService) {
            _authService = authService;
            _logger = logger;
            _userContextService = userContextService;
        }
        [HttpPost("login")]
        [SwaggerOperation("this end call use for the login of user")]
        [SwaggerResponse(statusCode:StatusCodes.Status200OK,Type =typeof(ResponseBase<Object>),Description ="this end call will generate the response which contains the success message and status code")]
        public async Task<ActionResult<AuthResponseDto>> LoginAsync(LoginRequestDto loginRequest)
        {
            _logger.LogInformation("user is trying to login from {UserIP}", _userContextService.GetUserIp());
            return (loginRequest.LMethod == LoginMethod.Password ? Ok(await _authService.LoginAsync(loginRequest)) : Ok(await _authService.LoginWithOtpAsync(loginRequest)));
        }

        [HttpPost("register/customer")]
        [SwaggerOperation("use this end call to register the customer")] 
        
        public async Task<ActionResult<ResponseBase<Object>>> RegisterCustomerAsync([FromBody] CustomerRegistrationDto customerRegistrationDto)
        {
            await _authService.RegisterCustomerAsync(customerRegistrationDto);
            return Ok(new ResponseBase<Object>(200,"customer registered successsfully"));
        }
        [HttpPost("register/staff")]
        [SwaggerOperation("use this end call to add the staff")]
        public async Task<ActionResult<ResponseBase<Object>>> RegisterStaffAsync([FromBody] StaffRegistrationDto staffRegistrationDto)
        {
            await _authService.RegisterStaffAsync(staffRegistrationDto);
            return Ok(new ResponseBase<Object>(200, "staff registered successfully"));
        }

        [HttpPost("get/refresh/token")]
        [SwaggerOperation("use this end call to get refresh token")]
        public async Task<ActionResult<AuthResponseDto>> RefreshTokenAsync([FromBody] RefreshTokenDto refreshToken)
        {
            return Ok(await _authService.RefreshTokenAsync(refreshToken));
        }

        [HttpPost("email/confirm")]
        [SwaggerOperation("use this end call to confirm the email of user")]
        public async Task<ActionResult<ResponseBase<Object>>> ConfirmEmail([FromBody] ConfirmEmailDto confirmEmailDto)
        {
            await _authService.ConfirmEmailAsync(confirmEmailDto);
            return Ok(new ResponseBase<Object>(StatusCodes.Status200OK, "Email confirmed successfully"));
        }
        [HttpPost("login/get/otp")]
        [SwaggerOperation("use this end call to get the otp for login")]
        public async Task<ActionResult<ResponseBase<Object>>> GenerateOtp([FromBody] AuthMailRequestDto otpRequestDto)
        {
            await _authService.LoginOtpSenderAsync(otpRequestDto);
            return Ok(new ResponseBase<Object>(StatusCodes.Status200OK, "Otp sent successfully"));
        }
        [HttpPost("get/email/confirm/token")]
        [SwaggerOperation("use this end call to get the email with confiramation token for the confirming email id")]
        public async Task<ActionResult<ResponseBase<Object>>> GetEmailConfirmToken([FromBody] AuthMailRequestDto authMailRequestDto)
        {
            await _authService.MailConfirmationSenderAsync(authMailRequestDto);
            return Ok(new ResponseBase<Object>(StatusCodes.Status200OK, "confirmation mail sent successfully"));
        }


    }
}
