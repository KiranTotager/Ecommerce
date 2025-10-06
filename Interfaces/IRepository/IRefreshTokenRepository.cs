using ECommerce.Models;

namespace ECommerce.Interfaces.IRepository
{
    public interface IRefreshTokenRepository
    {
        public Task AddAsync(RefreshToken refreshToken);
        public Task DeleteAsync(string refreshToken);
        public Task<RefreshToken> GetUserDetailsAsync(string refreshToken);
    }
}
