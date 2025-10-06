using ECommerce.CustomExceptions;
using ECommerce.Data;
using ECommerce.Interfaces.IRepository.IUserRepos;
using ECommerce.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Repository.UserRepos
{
    public class OrderedItemRepository : IOrderedItemRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrderedItemRepository> _logger;
        public OrderedItemRepository(ApplicationDbContext context, ILogger<OrderedItemRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task AddAsync(OrderedItem OItem)
        {
            try
            {
                await _context.OrderedItems.AddAsync(OItem);
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new ordered item.");
                throw;
            }
        }

        public async Task DeleteByIdAsync(long Id)
        {
            try
            {
                var OItem = await GetByIdAsync(Id);
                _context.OrderedItems.Remove(OItem);
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting an ordered item.");
                throw;
            }
        }

        public async Task<IEnumerable<OrderedItem>> GetAllAsync()
        {
            try
            {
               return await _context.OrderedItems.ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all ordered items.");
                throw;
            }
        }

        public async Task<OrderedItem> GetByIdAsync(long Id)
        {
            try
            {
                return await _context.OrderedItems.FindAsync(Id) ?? throw new NotFoundException("Ordered Item");
            }
            catch (NotFoundException nf)
            {
                _logger.LogError("Ordered Item with ID {Id} not found.", Id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the ordered item with ID {Id}.", Id);
                throw;
            }
        }

        public async Task UpdateAsync(OrderedItem OItem)
        {
            try
            {
                _context.OrderedItems.Update(OItem);
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the ordered item.");
                throw;
            }
        }
    }
}
