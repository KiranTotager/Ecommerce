using ECommerce.CustomExceptions;
using ECommerce.Data;
using ECommerce.Interfaces.IRepository.IUserRepos;
using ECommerce.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Repository.UserRepos
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartItemRepository> _logger;
        public CartItemRepository(ApplicationDbContext context, ILogger<CartItemRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task AddAsync(CartItem Item)
        {
            try
            {
                await _context.CartItems.AddAsync(Item);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding cart item : " + ex.Message);
                throw;
            }
        }

        public async Task DeleteByIdAsync(long Id)
        {
            try
            {
                CartItem? item = await _context.CartItems.FindAsync(Id);
                if (item==null)
                {
                    throw new NotFoundException("Cart ");
                }
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();

            }
            catch(NotFoundException nf)
            {
                _logger.LogError("Error deleting cart item : " + nf.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting cart item : " + ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<CartItem>> GetAllAsync()
        {
            try
            {
               return await _context.CartItems.ToListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching cart item : " + ex.Message);
                throw;
            }
            
        }

        public async Task<IEnumerable<CartItem>> GetByCustomerIdAsync(Guid customerId)
        {
            try
            {
                var Items = await _context.CartItems.Where(c => c.CustomerId == customerId)
                    .Include(Crt => Crt.Product)
                    .ThenInclude(Prdct=>Prdct.Ctgry)
                    .ToListAsync();
                if(Items==null || Items.Count==0)
                {
                    throw new NotFoundException("Cart items for the customer");
                }
                return Items;

            }
            catch(NotFoundException nf)
            {
                _logger.LogError("Error fetching cart item : " + nf.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching cart item : " + ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<CartItem>> GetByGuestIdAsync(Guid guestId)
        {
            try
            {
                var Items=await _context.CartItems.Where(c=>c.GuestId== guestId)
                    .Include(Crt=>Crt.Product)
                    .ThenInclude(Prdct=>Prdct.Ctgry)
                    .ToListAsync();
                if(Items==null || Items.Count==0)
                {
                    throw new NotFoundException("Cart items for the guest");
                }
                return Items;

            }
            catch(NotFoundException nf)
            {
                _logger.LogError("Error fetching cart item : " + nf.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching cart item : " + ex.Message);
                throw;
            }
        }

        public async Task<CartItem> GetByIdAsync(long Id)
        {
            try
            {
                var Item = await _context.CartItems.FindAsync(Id);
                if (Item == null)
                {
                    throw new NotFoundException("Cart item");
                }
                return Item;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching cart item : " + ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(CartItem Item)
        {
            try
            {
                 _context.CartItems.Update(Item);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Updating cart item : " + ex.Message);
                throw;
            }
        }
    }
}
