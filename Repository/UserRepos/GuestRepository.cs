using ECommerce.CustomExceptions;
using ECommerce.Data;
using ECommerce.Interfaces.IRepository.IUserRepos;
using ECommerce.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Repository.UserRepos
{
    public class GuestRepository : IGuestRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GuestRepository> _logger;
        public GuestRepository(ApplicationDbContext context,ILogger<GuestRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task AddAsync(Guest Gst)
        {
            try
            {
                await _context.Guests.AddAsync(Gst);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occurred while adding a new guest.");
                throw;
            }
        }

        public async Task DeleteByIdAsync(Guid Id)
        {
           // Soft delete implementation
        }

        public async Task<IEnumerable<Guest>> GetAllAsync()
        {
            try
            {
                return await _context.Guests.ToListAsync();
            }catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all guests.");
                throw;
            }
        }

        public async Task<Guest> GetByIdAsync(Guid Id)
        {
            try
            {
                return await _context.Guests.FindAsync(Id) ?? throw new NotFoundException("Guest");
            }catch(NotFoundException nf)
            {
                _logger.LogError("Guest with ID {Id} not found.", Id);
                throw;
            }catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the guest with ID {Id}.", Id);
                throw;
            }
        }

        public async Task<bool> IsGuestExist(Guid Id)
        {
            return await _context.Guests.AnyAsync(Gst=>Gst.Id == Id);
        }

        public async Task UpdateAsync(Guest Gst)
        {
            try
            {
                _context.Guests.Update(Gst);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a guest.");
                throw;
            }
        }
    }
}
