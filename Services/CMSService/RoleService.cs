using ECommerce.Models;
using Microsoft.AspNetCore.Identity;
using ECommerce.CustomExceptions;
using ECommerce.DTOs.Auth;
using Microsoft.EntityFrameworkCore;
using ECommerce.Interfaces.IServices.ICMSServices;
namespace ECommerce.Services.CMSService
{

    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RoleService> _logger;
        public RoleService(RoleManager<IdentityRole> roleManager,ILogger<RoleService> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
        }
        public async Task AddAsync(string Role)
        {
            if (await _roleManager.RoleExistsAsync(Role)) {
                throw new AlreadyExistException($"Role with the name {Role}");
            }
            await _roleManager.CreateAsync(new IdentityRole(Role));
        }

        public async Task DeleteAsync(string Role)
        {
            IdentityRole ExistRole=await _roleManager.FindByNameAsync(Role);
            if (ExistRole == null) {
                throw new NotFoundException($"role with the name {Role}");
            }
            var result=await _roleManager.DeleteAsync(ExistRole);
            if (!result.Succeeded)
            {
                throw new DeletionException("role");
            }
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var Roles =await _roleManager.Roles.ToListAsync();
            List<RoleDto> RoleDtos = Roles.Select(x => new RoleDto
            {
                Role = x.Name
            }).ToList();
            return RoleDtos;
        }
    }
}
