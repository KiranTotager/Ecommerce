using ECommerce.CustomExceptions;
using ECommerce.Data;
using ECommerce.Interfaces.IRepository.IUserRepos;
using ECommerce.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Repository.UserRepos
{
    public class WishListRepository : IWishListRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<WishListRepository> _logger;
        public WishListRepository(ApplicationDbContext context, ILogger<WishListRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task AddAsync(WishList WList)
        {
            try
            {
                await _context.WishLists.AddAsync(WList);
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new wishlist.");
                throw;
            }
        }

        public async Task DeleteAsync(long Id)
        {
            try
            {
                var WList = await GetByIdAsync(Id);
                _context.WishLists.Remove(WList);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("error while deleting the wish list of id "+ Id);
                throw;
            }
        }

        public async Task<IEnumerable<WishList>> GetAllAsync()
        {
            try
            {
                return await _context.WishLists
                    .Include(WL => WL.Prdct)
                    .ThenInclude(Prdct => Prdct.Ctgry)
                    .ToListAsync();
            }catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all wishlists.");
                throw;
            }
        }

        public async Task<IEnumerable<WishList>> GetByCustomerIdAsync(Guid CId)
        {
            try
            {
                return await _context.WishLists
                    .Where(w=>w.CustomerId==CId)
                    .Include(WL=>WL.Prdct)
                    .ThenInclude(Prdct=>Prdct.Ctgry)
                    .ToListAsync()?? throw new NotFoundException("Customer");
            }catch(NotFoundException nf)
            {
                _logger.LogError("Wishlists for Customer with ID {CId} not found.", CId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving wishlists by customer ID.");
                throw;
            }
        }

        public async Task<IEnumerable<WishList>> GetByGuestIdAsync(Guid GId)
        {
            try
            {
                return await _context.WishLists
                    .Where(w=>w.GuestId==GId)
                    .Include(WL=>WL.Prdct)
                    .ThenInclude(Prdct=>Prdct.Ctgry)
                    .ToListAsync()?? throw new NotFoundException("Guest");

            }catch(NotFoundException nf)
            {
                _logger.LogError("Wishlists for Guest with ID {GId} not found.", GId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving wishlists by guest ID.");
                throw;
            }
        }

        public async Task<WishList> GetByIdAsync(long Id)
        {
            try
            {
                return await _context.WishLists.FindAsync(Id) ?? throw new NotFoundException("Wishlist");
            }catch(NotFoundException nf)
            {
                _logger.LogError("Wishlist with ID {Id} not found.", Id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving a wishlist by ID.");
                throw;
            }
        }

        public async Task<WishList> GetWishListByCustomerIdAndProductIDAsync(Guid CustomerId, Guid ProductId)
        {
            return await _context.WishLists.FirstOrDefaultAsync(WL => WL.GuestId == CustomerId && WL.ProductsId == ProductId);
        }

        public async Task<WishList> GetWishListByGuestIdAndProductIDAsync(Guid GuestId, Guid ProductId)
        {
            return await _context.WishLists.FirstOrDefaultAsync(WL => WL.GuestId == GuestId && WL.ProductsId == ProductId);
        }

        public async Task UpdateAsync(WishList WList)
        {
            try
            {
                _context.WishLists.Update(WList);
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a wishlist.");
                throw;
            }
        }
    }
}
