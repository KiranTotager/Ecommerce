using ECommerce.CustomExceptions;
using ECommerce.Data;
using ECommerce.Interfaces.IRepository;
using ECommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RefreshTokenRepository> _logger;
        public RefreshTokenRepository(ApplicationDbContext context, ILogger<RefreshTokenRepository> logger)
        {
            _context = context;
           _logger = logger;
        }
        public async Task AddAsync(RefreshToken refreshToken)
        {
            try
            {
                await _context.RefreshTokens.AddAsync(refreshToken);
                await _context.SaveChangesAsync();
                _logger.LogInformation("refresh token saved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError("error while saving the refresh token");
                throw;
            }
        }

        public async Task DeleteAsync(string refreshToken)
        {
            try
            {
                RefreshToken ExistRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(refToken=>refToken.Token==refreshToken);
                if (ExistRefreshToken == null)
                { } 
                else 
                { 
                    _context.RefreshTokens.Remove(ExistRefreshToken);
                    await _context.SaveChangesAsync();

                }
            }
            catch (Exception ex) {
                _logger.LogError("error while deleting the refresh token");
                throw;
            }
        }

        public async Task<RefreshToken> GetUserDetailsAsync(string refreshToken)
        {
            try
            {
                return await _context.RefreshTokens.FirstOrDefaultAsync(refToken => refToken.Token == refreshToken) ?? throw new NotFoundException("refresh token");
            }
            catch (NotFoundException nf)
            {
                _logger.LogError("Not found "+nf.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("error while fetching the refresh token details because of " + ex.Message);
                throw;
            }
        }
    }
}
