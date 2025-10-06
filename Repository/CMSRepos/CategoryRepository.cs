using ECommerce.CustomExceptions;
using ECommerce.Data;
using ECommerce.Interfaces.IRepository.ICMSRepos;
using ECommerce.Models.CMSModels;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Repository.CMSRepos
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategoryRepository> _logger;
        public CategoryRepository(ApplicationDbContext context,ILogger<CategoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task AddAsync(Category Category)
        {
            try
            {
                await _context.AddAsync(Category);
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                _logger.LogError("Error adding category : " + ex.Message);
                throw;
            }
        }

        public async Task DeleteByIdAsync(int Id)
        {
            try
            {
                var Category = await _context.Categories.FindAsync(Id) ?? throw new NotFoundException($"Category of {Id} ");
                _context.Categories.Remove(Category);
                await _context.SaveChangesAsync();
            }
            catch (NotFoundException nf)
            {
                _logger.LogError("Error deleting category : " + nf.Message);
                throw;  
            }
            catch(Exception ex)
            {
                _logger.LogError("Error deleting category : " + ex.Message);
                throw;
            }
        }

        public async Task<Category> FindByNameAsync(string Name)
        {
            return await _context.Categories.FirstOrDefaultAsync(Ctgry=>Ctgry.Name== Name);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int Id)
        {
            return await _context.Categories.FindAsync(Id)?? throw new NotFoundException($"Category of {Id} ");
        }

        public async Task UpdateAsync(Category Category)
        {
            try
            {
                _context.Categories.Update(Category);
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                _logger.LogError("Error updating category : " + ex.Message);
                throw;
            }
        }
    }
}
