using ECommerce.CustomExceptions;
using ECommerce.DTOs.CMSDTOs;
using ECommerce.Interfaces.IRepository.ICMSRepos;
using ECommerce.Interfaces.IServices.ICMSServices;
using ECommerce.Interfaces.IUtils;
using ECommerce.Models.CMSModels;

namespace ECommerce.Services.CMSService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _prodcutRepository;
        private readonly IUserContextService _userConextService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IStaffRepository _staffRepository;
        public ProductService(IProductRepository productRepository,IUserContextService userConextService, ICategoryRepository categoryRepository, IStaffRepository staffRepository)
        {
            _prodcutRepository = productRepository;
            _userConextService = userConextService;
            _categoryRepository = categoryRepository;
            _staffRepository = staffRepository;
        }
        public async Task AddProductAsync(AddProductDto product)
        {
            Category DbCategory=await _categoryRepository.FindByNameAsync(product.ProductCategory) ?? throw new NotFoundException($"category with name {product.ProductCategory}");
            var UserId = _userConextService.GetUserId();
            Staff Currstaff = await _staffRepository.GetByIdAsync(UserId);
            Product NewProduct = new Product
            {
                Name = product.ProductName,
                Description = product.ProductDescription,
                Price = product.ProductPrice,
                Discount = product.ProductDiscount,
                StockQuantity = product.StockQuantity,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                AddedIp = _userConextService.GetUserIp(),
                CategoryId=DbCategory.Id
            };
            NewProduct.Staffs.Add( Currstaff );
            if (product.ProductImage != null)
            {
                var FileName = $"{Guid.NewGuid().ToString()}_{product.ProductImage.FileName}";
                var FilePath=Path.Combine("wwwroot/images",FileName);
                using(var stream=new FileStream(FilePath,FileMode.Create))
                {
                    await product.ProductImage.CopyToAsync(stream);
                }
                NewProduct.ImageUrl = $"images/{FileName}";
            }
            await _prodcutRepository.AddAsync(NewProduct);
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
        {
            IEnumerable<Product> Products = await _prodcutRepository.GetAllAsync();
            return Products.Select(Prdct => new ProductResponseDto
            {
                ProductId = Prdct.ProductId,
                ProductName = Prdct.Name,
                ProductCreatedAt = Prdct.CreatedAt,
                ProductUpdatedAt = Prdct.UpdatedAt.Value,
                ProductImageUrl = Prdct.ImageUrl,
                ProductAddedIp = Prdct.AddedIp,
                ProductCategoryName = Prdct.Ctgry.Name,
                ProductDescription = Prdct.Description,
                ProductPrice = Prdct.Price,
                ProductDiscount = Prdct.Discount ?? 0.0M,
                ProductStockQuantity = Prdct.StockQuantity,
                ProductAddedBy = Prdct.Staffs.Select(Stf => Stf.User.UserName).LastOrDefault() ?? ""
            });
        }
    }
}
