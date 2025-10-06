using ECommerce.CustomExceptions;
using ECommerce.Data;
using ECommerce.Interfaces.IRepository.IUserRepos;
using ECommerce.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Repository.UserRepos
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CustomerRepository> _logger;
        public CustomerRepository(ApplicationDbContext context, ILogger<CustomerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task AddAsync(Customer Cstmr)
        {
            try
            {
                await _context.Customers.AddAsync(Cstmr);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError("Error adding customer : " + ex.Message);
                throw;
            }
        }

        public async Task DeleteByIdAsync(Guid Id)
        {
            return;// because we are doing it as soft deletion
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            try
            {
                return await _context.Customers.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while fetching the customers " + ex.Message);
                throw;
            }
        }

        public async Task<Customer> GetByIdAsync(Guid Id)
        {
            try
            {
                return await _context.Customers.FirstOrDefaultAsync(Cstmr=>Cstmr.UserId==Id.ToString()) ?? throw new NotFoundException($"Customer of id {Id}");
            }
            catch(NotFoundException nf)
            {
                _logger.LogError($"Customer of id {Id} not found");
                throw;
            }
            catch (Exception ex) {
                _logger.LogError($"Could not find {Id}");
                throw;
            }
        }

        public async Task UpdateAsync(Customer Cstmr)
        {
            try
            {
                _context.Customers.Update(Cstmr);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) {
                _logger.LogError("exception while updating the customer");
                throw;
            }
        }
    }
}
