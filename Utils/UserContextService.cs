using ECommerce.CustomExceptions;
using ECommerce.Interfaces.IUtils;
using System.Security.Claims;

namespace ECommerce.Utils
{
    public class UserContextService : IUserConextService
    {
        private readonly IHttpContextAccessor _httpcontextAccessor;
        public UserContextService(IHttpContextAccessor httpcontextAccessor)
        {
            _httpcontextAccessor = httpcontextAccessor;
        }

        public string GetUserDevice()
        {
            return _httpcontextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }

        public Guid GetUserId()
        {
            var userId = _httpcontextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId!=null ?Guid.Parse(userId) :Guid.Empty;
        }

        public string GetUserIp()
        {
            return _httpcontextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        public string GetUserRole()
        {
            return _httpcontextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value ?? "";
        }

        public bool IsCustomer =>
            _httpcontextAccessor.HttpContext.User.Identity?.IsAuthenticated ?? false;
        public string? CustomerId =>
            _httpcontextAccessor.HttpContext.User.FindFirst("sub").Value;
        public string? GuestId =>
            _httpcontextAccessor.HttpContext.Request.Headers["GuestId"].FirstOrDefault();
    }
}
