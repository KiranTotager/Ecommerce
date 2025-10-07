using ECommerce.DTOs.CustomerDTOs;
using ECommerce.Enums;
using ECommerce.Interfaces.IRepository.ICMSRepos;
using ECommerce.Interfaces.IRepository.IUserRepos;
using ECommerce.Interfaces.IServices.ICustomerServices;
using ECommerce.Interfaces.IUtils;
using ECommerce.Models.CMSModels;
using ECommerce.Models.UserModels;

namespace ECommerce.Services.UserServices
{
    public class OrderService : IOrderService
    {
        private readonly IOrderDetailRepository _orderDetailRepsoitory;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IOrderedItemRepository _orderedItemRepository;
        private readonly IGuestRepository _guestRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserContextService _userContextService;
        private readonly ICoupenDetailRepository _coupenDetailRepository;

        public OrderService(ICartItemRepository cartItemRepository,IOrderedItemRepository orderedItemRepository,IGuestRepository guestRepository,ICustomerRepository customerRepository,IUserContextService userContextService,ICoupenDetailRepository coupenDetailRepository,
            IOrderDetailRepository orderDetailRepository)
        {
            _orderDetailRepsoitory = orderDetailRepository;
            _cartItemRepository = cartItemRepository;
            _orderedItemRepository = orderedItemRepository;
            _guestRepository = guestRepository;
            _customerRepository = customerRepository;
            _userContextService = userContextService;
            _coupenDetailRepository = coupenDetailRepository;
        }
        public async Task MakeOrderAsync(OrderRequestDto orderRequestDto)
        {
            CoupenDetail Coupen = await _coupenDetailRepository.FindByCCodeAsync(orderRequestDto.CoupenCode);
            if (_userContextService.IsCustomer)
            {
                // logic if the user is customer

                Guid AppCustomerId=_userContextService.GetUserId();
                Customer AppCustomer= await _customerRepository.GetByIdAsync(AppCustomerId);
                IEnumerable<CartItem> CustomerCartItems=await _cartItemRepository.GetByCustomerIdAsync(AppCustomerId);
                decimal OrderTotalPrice=CustomerCartItems.Sum(CrtItem=>(CrtItem.Product.Price * CrtItem.Quantity)-(CrtItem.Product.Discount ?? 0.0M*CrtItem.Quantity));
                if (Coupen.Status == CoupenStatus.Active)
                {
                    OrderTotalPrice -= Convert.ToDecimal(Coupen.CValue);
                }
                OrderDetail NewOrder = new OrderDetail
                {
                    TotalPrice = OrderTotalPrice,
                    FullName=orderRequestDto.ConsumerFullName,
                    Street=orderRequestDto.ConsumerStreet,
                    City=orderRequestDto.ConsumerCity,
                    State=orderRequestDto.ConsumerState,
                    PostalCode=orderRequestDto.ConsumerPostalCode,
                    PhooneNumber=orderRequestDto.ConsumerPhoneNumber,
                    Note=orderRequestDto.ConsumerNote,
                    CustomerId=AppCustomerId,
                    CreatedAt = DateTime.Now,
                    PaymentMethod = orderRequestDto.PaymentMethod,
                    PaymentStatus = PaymentStatus.Initiated,
                };
                await _orderDetailRepsoitory.AddAsync(NewOrder);
                AppCustomer.OrderDetails.Add(NewOrder);
                await _customerRepository.UpdateAsync(AppCustomer);
                await MakeOrderHelperAsync(CustomerCartItems, NewOrder);
            }
            else 
            {
                string GId = _userContextService.GuestId;
                Guid.TryParse(GId, out Guid GuestId);
                Guest AppGuest = await _guestRepository.GetByIdAsync(GuestId);
                IEnumerable<CartItem> GuestCartItems=await _cartItemRepository.GetByGuestIdAsync(GuestId);
                decimal TotalPrice = GuestCartItems.Sum(CrtItms => (CrtItms.Product.Price * CrtItms.Quantity) - (CrtItms.Product.Discount ?? 0.0M * CrtItms.Quantity));
                if (Coupen.Status == CoupenStatus.Active)
                {
                    TotalPrice -= Convert.ToDecimal(Coupen.CValue);
                }
                OrderDetail NewOrder = new OrderDetail
                {
                    TotalPrice = TotalPrice,
                    FullName = orderRequestDto.ConsumerFullName,
                    Street = orderRequestDto.ConsumerStreet,
                    City = orderRequestDto.ConsumerCity,
                    State = orderRequestDto.ConsumerState,
                    PostalCode = orderRequestDto.ConsumerPostalCode,
                    PhooneNumber = orderRequestDto.ConsumerPhoneNumber,
                    GuestId = AppGuest.Id,
                    PaymentMethod = orderRequestDto.PaymentMethod,
                    PaymentStatus = PaymentStatus.Initiated,
                    Note = orderRequestDto.ConsumerNote,
                    CreatedAt = DateTime.Now,
                };
                await MakeOrderHelperAsync(GuestCartItems, NewOrder);
                await _orderDetailRepsoitory.AddAsync(NewOrder);
            }
            
        }

        public async Task MakeOrderHelperAsync(IEnumerable<CartItem> cartItems, OrderDetail orderDetail)
        {
            foreach (CartItem CItem in cartItems)
            {
                OrderedItem NewOrderItem = new OrderedItem
                {
                    Quantity = CItem.Quantity,
                    Price = CItem.Product.Price,
                    Discount = CItem.Product.Discount,
                    CreatedAt = DateTime.Now,
                    Tax = 0,
                    OrderId = orderDetail.OrderId,
                    ProductId = CItem.ProductId,
                };
                await _orderedItemRepository.AddAsync(NewOrderItem);
            }
        }
    }
}
