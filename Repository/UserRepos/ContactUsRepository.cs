using ECommerce.CustomExceptions;
using ECommerce.Data;
using ECommerce.Interfaces.IRepository.IUserRepos;
using ECommerce.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Repository.UserRepos
{
    public class ContactUsRepository : IContactUsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ContactUsRepository> _logger;
        public ContactUsRepository(ApplicationDbContext context,ILogger<ContactUsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(ContactUs Contact)
        {
            try
            {
                await _context.AddAsync(Contact);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding contact us message : " + ex.Message);
                throw;
            }
        }

        public async Task DeleteByIdAsync(long Id)
        {
            try
            {
                var Contact = await _context.Contacts.FindAsync(Id) ?? throw new NotFoundException($"Contact us message of {Id} ");
                _context.Contacts.Remove(Contact);
                await _context.SaveChangesAsync();
            }
            catch(NotFoundException nf)
            {
                _logger.LogError("Error deleting contact us message : " + nf.Message);
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error deleting contact us message : " + ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<ContactUs>> GetAllAsync()
        {
            try
            {
                return await _context.Contacts.ToListAsync();
            }catch(Exception ex)
            {
                _logger.LogError("Error fetching contact us messages : " + ex.Message);
                throw;
            }
        }

        public async Task<ContactUs> GetByIdAsync(long Id)
        {
            try
            {
                return await _context.Contacts.FindAsync(Id) ?? throw new NotFoundException($"Contact us message of {Id} ");
            }
            catch (NotFoundException nf)
            {
                _logger.LogError("Error fetching contact us message : " + nf.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching contact us message : " + ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(ContactUs Contact)
        {
            try
            {
                _context.Contacts.Update(Contact);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating contact us message : " + ex.Message);
                throw;
            }
        }
    }
}
