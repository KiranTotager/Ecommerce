using ECommerce.DTOs.CustomerDTOs;
using ECommerce.Models.UserModels;

namespace ECommerce.Interfaces.IServices.ICustomerServices
{
    public interface IOrderService 
    {
        public Task MakeOrderAsync(OrderRequestDto orderRequestDto);
        public Task MakeOrderHelperAsync(IEnumerable<CartItem> cartItems, OrderDetail orderDetail);
    }
}
