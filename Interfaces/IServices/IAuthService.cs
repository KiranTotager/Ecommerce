using ECommerce.DTOs.Auth;
using ECommerce.Models;

namespace ECommerce.Interfaces.IServices
{
    public interface IAuthService
    {

        public Task<AuthResponseDto> LoginAsync(LoginRequestDto loginRequestDto); // this method will handle login logic with the password
        public Task RegisterCustomerAsync(CustomerRegistrationDto customerRegistrationDto);
        public Task RegisterStaffAsync(StaffRegistrationDto staffRegistrationDto);
        public Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto refreshToken);
        public Task ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto);
        public Task<AuthResponseDto> LoginWithOtpAsync(LoginRequestDto loginRequestDto);
        public Task<(string AuthToken, string RefreshToken)> LoginHelperAsync(ApplicationUser applicationUser);
        public Task LoginOtpSenderAsync(AuthMailRequestDto otpRequestDto);
        public Task MailConfirmationSenderAsync(AuthMailRequestDto confirmMailRequestDto);
    }
}
