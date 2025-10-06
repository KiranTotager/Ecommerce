using ECommerce.CustomExceptions;
using ECommerce.Data;
using ECommerce.Interfaces.IRepository.ICMSRepos;
using ECommerce.Models.CMSModels;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Repository.CMSRepos
{
    public class StaffRepository : IStaffRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StaffRepository> _logger;
        public StaffRepository(ApplicationDbContext context, ILogger<StaffRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(Staff Stf)
        {
            try
            {
                await _context.Staffs.AddAsync(Stf);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new staff member.");
                throw;
            }
        }

        public async Task DeleteByIdAsync(Guid Id)
        {
            //soft delete can be implemented here
        }

        public async Task<IEnumerable<Staff>> GetAllAsync()
        {
            try
            {
                return await _context.Staffs.ToListAsync(); 
            }catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all staff members.");
                throw;
            }
        }

        public async Task<Staff> GetByIdAsync(Guid Id)
        {
            try
            {
                _logger.LogInformation("user id in repo is"+Id);
                return await _context.Staffs.FirstOrDefaultAsync(Stf=>Stf.User.Id==Id.ToString()) ?? throw new NotFoundException("staff");
            }catch(NotFoundException nf)
            {
                _logger.LogError("Staff member with ID {Id} not found.", Id);
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the staff member.");
                throw;
            }
        }

        public async Task UpdateAsync(Staff Stf)
        {
            try
            {
                _context.Staffs.Update(Stf);
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the staff member.");
                throw;
            }
        }
    }
}
