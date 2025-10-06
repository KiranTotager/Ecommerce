using ECommerce.CustomExceptions;
using ECommerce.DTOs.CustomerDTOs;
using ECommerce.Interfaces.IRepository.ICMSRepos;
using ECommerce.Interfaces.IRepository.IUserRepos;
using ECommerce.Interfaces.IServices.ICustomerServices;
using ECommerce.Interfaces.IUtils;
using ECommerce.Models.CMSModels;
using ECommerce.Models.UserModels;

namespace ECommerce.Services.UserServices
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IUserConextService _userConextService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRespository;
        private readonly IGuestRepository _guestRespository;
        private readonly ILogger<CartItemService> _logger;
        public CartItemService(ICartItemRepository cartItemRepository, IUserConextService userConextService, IHttpContextAccessor contextAccessor,ICustomerRepository customerRepository,IProductRepository productRepository,IGuestRepository guestRepository,ILogger<CartItemService> logger)
        {
            _cartItemRepository = cartItemRepository;
            _userConextService = userConextService;
            _contextAccessor = contextAccessor;
            _customerRepository = customerRepository;
            _productRespository = productRepository;
            _guestRespository= guestRepository;
            _logger = logger;
        }

        public async Task AddToCartAsync(AddCartDTO addCartDTO)
        {
            if (!await _productRespository.IsProductExistAsync(addCartDTO.ProductId)) {
                throw new NotFoundException($"product with the id {addCartDTO.ProductId}");
            }
            if (_userConextService.IsCustomer)
            {
                _logger.LogInformation("inside the customer section");
                var CustomerId=_userConextService.GetUserId();
                if (CustomerId==Guid.Empty)
                {
                    throw new SecurityTokenException("token is invalid");
                }
                Customer CurrentCustomer=await _customerRepository.GetByIdAsync(CustomerId);
                Product CartProduct=await _productRespository.GetByIdAsync(addCartDTO.ProductId);
                CartItem NewCartItem = new CartItem
                {
                    Quantity = addCartDTO.Quantity,
                    CustomerId=CurrentCustomer.Id,
                    AddedAt=DateTime.Now,
                    ProductId=CartProduct.ProductId,
                };
                await _cartItemRepository.AddAsync(NewCartItem);
            }
            else
            {
                Guid guestId;
                Guid.TryParse(_userConextService.GuestId, out guestId);
                Guest AGuest;
                try
                {
                   AGuest=await _guestRespository.GetByIdAsync(guestId);
                }catch(NotFoundException nf)
                {
                    _logger.LogInformation("guest id not found ,creating new one");
                    AGuest = new Guest();
                    await _guestRespository.AddAsync(AGuest);
                    guestId = AGuest.Id;
                    _contextAccessor.HttpContext.Response.Cookies.Append(
                        "GuestId",
                        guestId.ToString().ToUpper(),
                        new CookieOptions
                        {
                            HttpOnly = false,
                            Expires = DateTimeOffset.Now.AddDays(30)
                        }
                    );
                }
                Product CartProduct = await _productRespository.GetByIdAsync(addCartDTO.ProductId);
                CartItem NewCartItem = new CartItem
                {
                    Quantity = addCartDTO.Quantity,
                    GuestId = guestId,
                    AddedAt = DateTime.Now,
                    ProductId = CartProduct.ProductId,
                };
                await _cartItemRepository.AddAsync(NewCartItem);
            }
        }

        public async Task<IEnumerable<CartItemResponseDto>> GetAllCartItemsAsync()
        {
            IEnumerable<CartItem> CartItems;
            if (_userConextService.IsCustomer)
            {
                Guid AppCustomerId=_userConextService.GetUserId();
                CartItems = await _cartItemRepository.GetByCustomerIdAsync(AppCustomerId);
            }
            else
            {
                Guid AppGuestId=Guid.Parse(_userConextService.GuestId);
                _logger.LogInformation("the guest id recieved is "+AppGuestId);
                CartItems=await _cartItemRepository.GetByGuestIdAsync(AppGuestId);
            }
            return CartItems.Select(CrtItms => new CartItemResponseDto
            {
                ProductId = CrtItms.ProductId,
                ProductQuantity = CrtItms.Quantity,
                ProductName=CrtItms.Product.Name,
                ProductDescription=CrtItms.Product.Description,
                ProductPrice=CrtItms.Product.Price,
                ProductDiscount=CrtItms.Product.Discount ?? 0.0M,
                ProductImageUrl=CrtItms.Product.ImageUrl,
                ProductCategory = CrtItms.Product.Ctgry.Name
            }).ToList();
        }
    }
}
