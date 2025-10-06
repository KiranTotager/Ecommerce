using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace ECommerce.Interfaces.IUtils
{
    public interface IUserConextService
    {
        public string? CustomerId { get; }
        public string? GuestId { get; }
        public bool IsCustomer { get;}
        public Guid GetUserId();
        public string GetUserRole();
        public string GetUserIp();
        public string GetUserDevice();
    }
}
