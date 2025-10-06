using ECommerce.CustomExceptions;
using ECommerce.Data;
using ECommerce.DTOs.Auth;
using ECommerce.Enums;
using ECommerce.Interfaces.IRepository;
using ECommerce.Interfaces.IRepository.ICMSRepos;
using ECommerce.Interfaces.IRepository.IUserRepos;
using ECommerce.Interfaces.IServices;
using ECommerce.Interfaces.IUtils;
using ECommerce.Models;
using ECommerce.Models.CMSModels;
using ECommerce.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace ECommerce.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthService> _logger;
        private readonly IConfiguration _configuration;
        private readonly ICustomerRepository _customerRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly IMailService _mailService;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserConextService _userConextService;

        public AuthService(ITokenService jwtService, UserManager<ApplicationUser> userManager,IRefreshTokenRepository refreshTokenRepository, IHttpContextAccessor httpContextAccessor, ILogger<AuthService> logger, IConfiguration configuration, ICustomerRepository customerRepository, IMailService mailService, IStaffRepository staffRepository, RoleManager<IdentityRole> roleManager, ICartItemRepository cartItemRepository, IUserConextService userConextService)
        {
            _tokenService = jwtService;
            _userManager = userManager;
            _refreshTokenRepository = refreshTokenRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _configuration = configuration;
            _customerRepository = customerRepository;
            _mailService = mailService;
            _staffRepository = staffRepository;
            _roleManager = roleManager;
            _cartItemRepository = cartItemRepository;
            _userConextService = userConextService;
        }

        public async Task ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto)
        {
            ApplicationUser AppUser=await _userManager.FindByIdAsync(confirmEmailDto.UserId)??throw new NotFoundException($"user with id {confirmEmailDto.UserId}");
            var originalToken = WebUtility.UrlDecode(confirmEmailDto.token);
            originalToken = WebUtility.UrlDecode(originalToken);
            var result = await _userManager.ConfirmEmailAsync(AppUser, originalToken);
            if (!result.Succeeded)
            {
                throw new SecurityTokenException("invalid token");
            }
            return;

        }


        // login logic -->
        // this method will handle login logic with the password
        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            ApplicationUser applicationUser = await _userManager.FindByEmailAsync(loginRequestDto.EmailId) ?? throw new NotFoundException("user");
            bool isValidPassword=await _userManager.CheckPasswordAsync(applicationUser, loginRequestDto.Credential);
            if (!applicationUser.EmailConfirmed)
            {
                throw new EmailNotConfirmedException(applicationUser.Email);
            }
            else if (!isValidPassword)
            {
                throw new UnauthorizedAccessException("invalid password");
            }
            var (AuthToken, RefreshToken) =await LoginHelperAsync(applicationUser);
            return new AuthResponseDto { UserId = applicationUser.Id, AuthToken = AuthToken,RefreshToken=RefreshToken};

        }
        //login with the otp method logic
        public async Task<AuthResponseDto> LoginWithOtpAsync(LoginRequestDto loginRequestDto)
        {
            ApplicationUser AppUser= await _userManager.FindByEmailAsync(loginRequestDto.EmailId)?? throw new NotFoundException("user");
            if (!AppUser.EmailConfirmed)
            {
                throw new EmailNotConfirmedException(AppUser.Email);
            }
            var result =await _userManager.VerifyTwoFactorTokenAsync(AppUser,"Email",loginRequestDto.Credential);
            if (!result)
            {
                throw new SecurityTokenException("otp is not valid");
            }
            var (AuthToken,RefreshToken) = await LoginHelperAsync(AppUser);
            return new AuthResponseDto { UserId = AppUser.Id, AuthToken = AuthToken,RefreshToken=RefreshToken};

        }

        public async Task<(string AuthToken,string RefreshToken)> LoginHelperAsync(ApplicationUser applicationUser)
        {
            int expiryTime = _configuration.GetValue<int>("Jwt:RefreshTokenExpiryTime");
            applicationUser.LastLogin = DateTime.Now;
            if (string.Equals(applicationUser.UserType, "Customer"))
            {
                string GuestId = _userConextService.GuestId;
                Customer Cstmr = await _customerRepository.GetByIdAsync(Guid.Parse(applicationUser.Id));
                if (!string.IsNullOrEmpty(GuestId))
                {
                    IEnumerable<CartItem> CustomerCarts = await _cartItemRepository.GetByGuestIdAsync(Guid.Parse(GuestId));
                    foreach (CartItem CrtItm in CustomerCarts)
                    {
                        CrtItm.GuestId = null;
                        CrtItm.CustomerId = Cstmr.Id;
                        await _cartItemRepository.UpdateAsync(CrtItm);
                    }
                }
            }
            await _userManager.UpdateAsync(applicationUser);
            string AuthToken = await _tokenService.GenerateJWTTokenAsync(applicationUser);
            string RefreshToken = _tokenService.GenerateRefreshToken();
            string currentIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
            string currentDevice = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
            DateTime expireTime = DateTime.UtcNow.AddDays(expiryTime);
            await _refreshTokenRepository.AddAsync(new RefreshToken { UserId = applicationUser.Id, Token = RefreshToken, ExpiryDate = expireTime, DeviceInfo = currentDevice, IPAddress = currentIp, IsRevoked = false });
            return (AuthToken,RefreshToken);
        }

        
        

        // refresh token logic-->

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto refreshToken)
        {
            RefreshToken DbRefreshToken=await _refreshTokenRepository.GetUserDetailsAsync(refreshToken.RefreshToken);
            var currentIP=_httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
            var currentDevice = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
            int expiryTime = _configuration.GetValue<int>("Jwt:RefreshTokenExpiryTime");
            try
            {
                if (DbRefreshToken.ExpiryDate <= DateTime.UtcNow || DbRefreshToken.IsRevoked)
                {
                    throw new SecurityTokenException("invalid refresh token ");

                }
                else if (!string.Equals(currentIP, DbRefreshToken.IPAddress))
                {
                    throw new SecurityTokenException("Ip address MisMatch ");
                }
                else if (!string.Equals(currentDevice, DbRefreshToken.DeviceInfo))
                {
                    throw new SecurityTokenException("Device details MisMatch");
                }
            }
            catch (SecurityTokenException ex) {
                _logger.LogInformation("refresh token is not valid");
                await _refreshTokenRepository.DeleteAsync(refreshToken.RefreshToken);
                throw;
            }catch(Exception ex)
            {
                _logger.LogError($"error : {ex.Message}");
                throw;
            }
            var User=await _userManager.FindByIdAsync(DbRefreshToken.UserId)?? throw new NotFoundException("user");
            string authToken = await _tokenService.GenerateJWTTokenAsync(User);
            string newRefToken = _tokenService.GenerateRefreshToken();
            DateTime expireTime = DateTime.UtcNow.AddDays(expiryTime);
            await _refreshTokenRepository.DeleteAsync(refreshToken.RefreshToken);
            await _refreshTokenRepository.AddAsync(new RefreshToken { UserId = User.Id, Token = newRefToken, ExpiryDate = expireTime, DeviceInfo = currentDevice, IPAddress = currentIP,IsRevoked=false });
            return new AuthResponseDto {UserId=DbRefreshToken.UserId ,AuthToken = authToken,RefreshToken=newRefToken};
            
            
        }


        // registering the customer logic
        public async Task RegisterCustomerAsync(CustomerRegistrationDto customerRegistrationDto)
        {
            string domain = _configuration.GetValue<string>("MailSettings:domain");
            try
            {
                var User = await _userManager.FindByEmailAsync(customerRegistrationDto.EmailId);
                if (User != null)
                {
                    throw new AlreadyExistException($"user with email id {customerRegistrationDto.EmailId}");
                }
                ApplicationUser NewUser = new ApplicationUser
                {
                    PublicUserName = customerRegistrationDto.UserName,
                    UserName = customerRegistrationDto.EmailId,
                    Email = customerRegistrationDto.EmailId,
                    PhoneNumber = customerRegistrationDto.PhoneNumber,
                    Address = customerRegistrationDto.Address,
                    UserType = UserType.Customer,
                };
                var result = await _userManager.CreateAsync(NewUser, customerRegistrationDto.Password);
                string mailConfirmationToken = await _tokenService.GenerateMailVerificationTokenAsync(NewUser);
                string url = $"{domain}?userId={NewUser.Id}&token={WebUtility.UrlEncode(mailConfirmationToken)}";


                if (!result.Succeeded)
                {
                    var Errors = string.Join(",", result.Errors.Select(e => e.Description));
                    throw new CreationException(Errors);
                }
                Customer AppCustomer = new Customer
                {
                    UserId = NewUser.Id,
                };
                await _customerRepository.AddAsync(AppCustomer);
                await _mailService.ConfirmationTokenSender(NewUser.Email, url);
            }
            catch
            {
                var User =await _userManager.FindByEmailAsync(customerRegistrationDto.EmailId);
                if(User != null)
                {
                    await _userManager.DeleteAsync(User);
                }
                throw;
            }
            
           
        }

        public async Task RegisterStaffAsync(StaffRegistrationDto staffRegistrationDto)
        {
            string domain = _configuration.GetValue<string>("MailSettings:domain");
            try
            {
                if(!await _roleManager.RoleExistsAsync(staffRegistrationDto.Role))
                {
                    throw new NotFoundException($"role with name {staffRegistrationDto.Role}");
                }
                var User = await _userManager.FindByEmailAsync(staffRegistrationDto.EmailId);
                if (User != null)
                {
                    throw new AlreadyExistException($"staff with the email id {staffRegistrationDto.EmailId}");
                }
                ApplicationUser ApplicationStaff = new ApplicationUser
                {
                    PublicUserName=staffRegistrationDto.UserName,
                    UserName = staffRegistrationDto.EmailId,
                    Email = staffRegistrationDto.EmailId,
                    PhoneNumber = staffRegistrationDto.PhoneNumber,
                    Address = staffRegistrationDto.Address,
                };
                var result = await _userManager.CreateAsync(ApplicationStaff, staffRegistrationDto.Password);
                if (!result.Succeeded)
                {
                    foreach(var eror in result.Errors)
                    {
                        _logger.LogInformation("exceptions thrown is " + eror.Description);

                    }
                    var filteredEerrors = result.Errors.Where(e => !e.Code.Contains("DuplicateUserName"));
                    if (filteredEerrors.Any())
                    {
                        var Errors = string.Join(",", result.Errors.Select(e => e.Description));
                        throw new CreationException(Errors);
                    }
                    throw new CreationException("error while creating the staff because of " + filteredEerrors);
                }
                await _userManager.AddToRoleAsync(ApplicationStaff,staffRegistrationDto.Role);
                string mailConfirmationToken = await _tokenService.GenerateMailVerificationTokenAsync(ApplicationStaff);
                string url = $"{domain}?userId={ApplicationStaff.Id}&token={WebUtility.UrlEncode(mailConfirmationToken)}";
               
                Staff ApplicationStaffExtraDetails = new Staff
                {
                    UserId = ApplicationStaff.Id,
                };
                await _staffRepository.AddAsync(ApplicationStaffExtraDetails);
                await _mailService.ConfirmationTokenSender(ApplicationStaff.Email, url);
            }
            catch {
                var user = await _userManager.FindByEmailAsync(staffRegistrationDto.EmailId);
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                }
                throw;
            }
        }
        
        //logic to send the login otp
        public async Task LoginOtpSenderAsync(AuthMailRequestDto otpRequestDto)
        {
            ApplicationUser AppUser = await _userManager.FindByEmailAsync(otpRequestDto.EmailId) ?? throw new NotFoundException($"user with email id {otpRequestDto.EmailId}");
            if (!AppUser.EmailConfirmed) {
                throw new EmailNotConfirmedException(AppUser.Email);
            }
            var otp=await _userManager.GenerateTwoFactorTokenAsync(AppUser,"Email");
            await _mailService.OTPSender(otpRequestDto.EmailId, otp);
        }

        public async Task MailConfirmationSenderAsync(AuthMailRequestDto confirmMailRequestDto)
        {
            string domain = _configuration.GetValue<string>("MailSettings:domain");
            ApplicationUser AppUser = await _userManager.FindByEmailAsync(confirmMailRequestDto.EmailId) ?? throw new NotFoundException($"user with the email id {confirmMailRequestDto.EmailId}");
            string mailConfirmationToken = await _tokenService.GenerateMailVerificationTokenAsync(AppUser);
            string url = $"{domain}?userId={AppUser.Id}&token={WebUtility.UrlEncode(mailConfirmationToken)}";
            await _mailService.ConfirmationTokenSender(AppUser.Email, url);
        }
    }
}
