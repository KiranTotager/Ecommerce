using ECommerce.Models;

namespace ECommerce.Interfaces.IUtils
{
    public interface ITokenService
    {
        public Task<string> GenerateJWTTokenAsync(ApplicationUser applicationUser);
        public string GenerateRefreshToken();
        public Task<string?> GenerateMailVerificationTokenAsync(ApplicationUser applicationUser);
    }
}
