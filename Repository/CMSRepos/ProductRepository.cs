using ECommerce.CustomExceptions;
using ECommerce.Data;
using ECommerce.Interfaces.IRepository.ICMSRepos;
using ECommerce.Models.CMSModels;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Repository.CMSRepos
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductRepository> _logger;
        public ProductRepository(ApplicationDbContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task AddAsync(Product Prdct)
        {
            try
            {
                await _context.Products.AddAsync(Prdct);
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new product.");
                throw;
            }
        }

        public Task DeleteByIdAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                return await _context.Products
                    .Include(Prdct =>Prdct.Ctgry )
                    .Include(Prdct=>Prdct.Staffs)
                    .ThenInclude(Stfs=>Stfs.User)
                    .ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all products.");
                throw;
            }
        }

        public async Task<Product> GetByIdAsync(Guid Id)
        {
            try
            {
                return await _context.Products.FindAsync(Id) ?? throw new NotFoundException("Product");
            }
            catch (NotFoundException nf)
            {
                _logger.LogError("Product with ID {Id} not found.", Id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the product with ID {Id}.", Id);
                throw;
            }
        }

        public async Task<bool> IsProductExistAsync(Guid Id)
        {
            return await _context.Products.AnyAsync(Prdct=>Prdct.ProductId == Id);
        }

        public async Task UpdateAsync(Product Prdct)
        {
            try
            {
                _context.Products.Update(Prdct);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the product.");
                throw;
            }
        }
    }
}
