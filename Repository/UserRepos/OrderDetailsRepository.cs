using ECommerce.CustomExceptions;
using ECommerce.Data;
using ECommerce.Interfaces.IRepository.IUserRepos;
using ECommerce.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Repository.UserRepos
{
    public class OrderDetailsRepository : IOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrderDetailsRepository> _logger;
        public OrderDetailsRepository(ApplicationDbContext context, ILogger<OrderDetailsRepository> logger)
        {
            _context=context;
            _logger=logger;
        }
        public async Task AddAsync(OrderDetail Order)
        {
            try
            {
                await _context.OrderDetails.AddAsync(Order);

            }catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new order detail.");
                throw;
            }
        }

        public async Task DeleteByIdAsync(Guid Id)
        {
            try
            {
               var OrderDetail= await GetByIdAsync(Id); // to check if it exists
                _context.OrderDetails.Remove(OrderDetail);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting an order detail.");
                throw;
            }
        }

        public async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            try
            {
                return await _context.OrderDetails.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all order details.");
                throw;
            }
        }

        public async Task<OrderDetail> GetByIdAsync(Guid Id)
        {
            try
            {
                return await _context.OrderDetails.FindAsync(Id) ?? throw new NotFoundException("Order detail");
            } catch (NotFoundException nf)
            {
                _logger.LogError("Order detail with ID {Id} not found.", Id);
                throw;
            } catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while Fetching the order detail with ID {Id}.", Id);
                throw;
            }
        }

        public async Task UpdateAsync(OrderDetail Order)
        {
            try
            {
                _context.OrderDetails.Update(Order);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating an order detail.");
                throw;
            }
        }
    }
}
