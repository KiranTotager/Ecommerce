using ECommerce.DTOs.Auth;

namespace ECommerce.Interfaces.IServices.ICMSServices
{
    public interface IRoleService
    {
        public Task AddAsync(string Role);
        public Task DeleteAsync(string Role);
        public Task<IEnumerable<RoleDto>> GetAllRolesAsync();
    }
}
