using ECommerce.CustomExceptions;
using ECommerce.Data;
using ECommerce.Interfaces.IRepository.IUserRepos;
using ECommerce.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Repository.UserRepos
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReviewRepository> _logger;
        public ReviewRepository(ApplicationDbContext context, ILogger<ReviewRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task AddAsync(Review Rvw)
        {
            try
            {
                await _context.Reviews.AddAsync(Rvw);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new review.");
                throw;
            }
        }

        public async Task DeleteByIdAsync(long Id)
        {
            try
            {
                var Rvw = await GetByIdAsync(Id);
                _context.Reviews.Remove(Rvw);
                await _context.SaveChangesAsync();
            }catch(Exception ex){
                _logger.LogError(ex, "An error occurred while deleting a review.");
            }
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            try
            {
                return await _context.Reviews.ToListAsync();
            }catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all reviews.");
                throw;
            }
        }

        public async Task<Review> GetByIdAsync(long Id)
        {
            try
            {
                return await _context.Reviews.FindAsync(Id) ?? throw new NotFoundException("Review");
            }catch(NotFoundException nf)
            {
                _logger.LogError("Review with ID {Id} not found.", Id);
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the review.");
                throw;
            }
        }

        public async Task<IEnumerable<Review>> GetByProductIdAsync(Guid productId)
        {
            try
            {
                return await _context.Reviews.Where(r => r.ProductId == productId).ToListAsync();
            }catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving reviews for product ID {productId}.", productId);
                throw;
            }
        }

        public async Task UpdateAsync(Review Rvw)
        {
            try
            {
                _context.Reviews.Update(Rvw);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the review.");
                throw;
            }
        }
    }
}
