using ECommerce.CustomExceptions;
using ECommerce.DTOs.CustomerDTOs;
using ECommerce.Interfaces.IRepository.ICMSRepos;
using ECommerce.Interfaces.IRepository.IUserRepos;
using ECommerce.Interfaces.IServices.ICustomerServices;
using ECommerce.Interfaces.IUtils;
using ECommerce.Models.CMSModels;
using ECommerce.Models.UserModels;
using System.Reflection.Metadata.Ecma335;

namespace ECommerce.Services.UserServices
{
    public class WishListService : IWishListService
    {
        private readonly IWishListRepository _repository;
        private readonly IUserConextService _userConextService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IGuestRepository _guestRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<WishListService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public WishListService(IWishListRepository repository, IUserConextService userConextService, ICustomerRepository customerRepository, IGuestRepository guestRepository,IProductRepository productRepository,ILogger<WishListService> logger,IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _userConextService = userConextService;
            _customerRepository = customerRepository;
            _guestRepository = guestRepository;
            _productRepository = productRepository;
            _logger = logger;
            _httpContextAccessor= httpContextAccessor;
        }

        public async Task AddToWishListAsync(AddWishListDto addWishListDto)
        {
            if (!await _productRepository.IsProductExistAsync(addWishListDto.ProductId)) {
                throw new NotFoundException($"product with id {addWishListDto.ProductId}");
            }
            if (_userConextService.IsCustomer)
            {
                Guid CustmorId=_userConextService.GetUserId();
                Product DbProduct = await _productRepository.GetByIdAsync(addWishListDto.ProductId);
                Customer AppCustomer =await _customerRepository.GetByIdAsync(CustmorId);
                WishList NewWishList = new WishList
                {
                    ProductsId=addWishListDto.ProductId,
                    CustomerId = AppCustomer.Id,

                };
                await _repository.AddAsync(NewWishList);
            }
            else
            {
                Guid GuestID;
                Guid.TryParse(_userConextService.GuestId, out GuestID);
                Product DbProduct =await _productRepository.GetByIdAsync(addWishListDto.ProductId);
                try
                {
                    Guest AppGuest = await _guestRepository.GetByIdAsync(GuestID);
                }
                catch (NotFoundException nf)
                {
                    _logger.LogInformation($"guest is with the id {GuestID} not exist ,creating new one");
                    Guest NewGuest = new Guest();
                    GuestID = NewGuest.Id;
                }
                WishList NewWishList = new WishList
                {
                    ProductsId=addWishListDto.ProductId,
                    GuestId = GuestID,
                };
                await _repository.AddAsync(NewWishList);
            }
                
        }

        public async Task<IEnumerable<WishListResponseDto>> GetAllWishListsOfUserAsync()
        {
            IEnumerable<WishList> WishLists;
            if (_userConextService.IsCustomer)
            {
                Guid CustomerId=_userConextService.GetUserId() ;
                WishLists=await _repository.GetByCustomerIdAsync(CustomerId);
            }
            else
            {
                string GuestId = _userConextService.GuestId;
                if (string.IsNullOrEmpty(GuestId))
                {
                    return new List<WishListResponseDto>();
                }
                Guid AGuestId =Guid.Parse(GuestId);
                WishLists=await _repository.GetByGuestIdAsync(AGuestId);
            }
                return WishLists.Select(WL =>
                    new WishListResponseDto
                    {
                        WishListId = WL.Id,
                        ProductAddedAt = WL.AddedAt,
                        ProductName = WL.Prdct.Name,
                        ProductId = WL.ProductsId,
                        ProductDescription = WL.Prdct.Description,
                        ProductPrice = WL.Prdct.Price,
                        ProductDiscount = WL.Prdct.Discount ?? 0.0M,
                        ProductCategory = WL.Prdct.Ctgry.Name,
                        ProductImageUrl = WL.Prdct.ImageUrl
                    }
                );
        }
    }
}
