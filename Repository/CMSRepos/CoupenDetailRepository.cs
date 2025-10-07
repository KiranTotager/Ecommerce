using ECommerce.CustomExceptions;
using ECommerce.Data;
using ECommerce.Interfaces.IRepository.ICMSRepos;
using ECommerce.Models.CMSModels;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Repository.CMSRepos
{
    public class CoupenDetailRepository : ICoupenDetailRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CoupenDetailRepository> _logger;
        public CoupenDetailRepository(ApplicationDbContext context,ILogger<CoupenDetailRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(CoupenDetail Coupen)
        {
            try
            {
                await _context.CoupenDetails.AddAsync(Coupen);
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                _logger.LogError("Error adding coupen detail : " + ex.Message);
                throw;
            }
        }

        public async Task DeleteByIdAsync(int Id)
        {
            try
            {
                var Coupen= await _context.CoupenDetails.FindAsync(Id) ?? throw new NotFoundException("Coupen");
                _context.CoupenDetails.Remove(Coupen);
                await _context.SaveChangesAsync();

            }catch(NotFoundException nf)
            {
                _logger.LogError("Error deleting coupen detail : " + nf.Message);
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error deleting coupen detail : " + ex.Message);
                throw;
            }
        }

        public async Task<CoupenDetail> FindByCCodeAsync(string CCode)
        {
           return await _context.CoupenDetails.FirstOrDefaultAsync(CPN=>CPN.CCode==CCode);
        }

        public async Task<IEnumerable<CoupenDetail>> GetActiveCoupensAsync()
        {
            return await _context.CoupenDetails.Where(CpnDtl=>CpnDtl.Status==Enums.CoupenStatus.Active).ToListAsync();
        }

        public async Task<IEnumerable<CoupenDetail>> GetAllAsync()
        {
            try
            {
                return await _context.CoupenDetails.ToListAsync();
            }catch(Exception ex)
            {
                _logger.LogError("Error fetching coupen details : " + ex.Message);
                throw;
            }
        }

        public async Task<CoupenDetail> GetByIdAsync(int Id)
        {
            try
            {
                return await _context.CoupenDetails.FindAsync(Id) ?? throw new NotFoundException($"Coupen of id {Id}");
            }catch(NotFoundException nf)
            {
                _logger.LogError("Error fetching coupen detail : " + nf.Message);
                throw;
            }catch(Exception ex)
            {
                _logger.LogError("Error fetching coupen detail : " + ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(CoupenDetail Coupen)
        {
            try
            {
                _context.CoupenDetails.Update(Coupen);
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                _logger.LogError("Error while updating coupen detail : " + ex.Message);
                throw;
            }
        }
    }
}
