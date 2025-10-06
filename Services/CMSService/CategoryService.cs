using ECommerce.CustomExceptions;
using ECommerce.DTOs.CMSDTOs;
using ECommerce.Interfaces.IRepository.ICMSRepos;
using ECommerce.Interfaces.IServices.ICMSServices;
using ECommerce.Interfaces.IUtils;
using ECommerce.Models.CMSModels;

namespace ECommerce.Services.CMSService
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly ILogger<CategoryService> _logger;
        private readonly IUserConextService _userConextService;
        private readonly IStaffRepository _staffRepository;
        public CategoryService(ICategoryRepository repository,ILogger<CategoryService> logger,IUserConextService userConextService,IStaffRepository staffRepository)
        {
            _repository = repository;
            _logger = logger;
            _userConextService = userConextService;
            _staffRepository = staffRepository;
        }
        public async Task AddCategoryAsync(AddCategoryDto addCategoryDto)
        {
            Category Ctgry=await _repository.FindByNameAsync(addCategoryDto.Name.ToLower());
            if (Ctgry != null)
            {
                throw new AlreadyExistException($"category with the name {addCategoryDto.Name}");
            }
            Guid staffId =_userConextService.GetUserId();
            _logger.LogInformation("the authenticated user id is " + staffId);
            if (staffId == null)
            {
                throw new SecurityTokenException("invalid or missing authentication token");
            }
            Staff Stf =await _staffRepository.GetByIdAsync(staffId);
            Category newCategory = new Category
            {
                Name = addCategoryDto.Name.ToLower(),
                DisplayHome=addCategoryDto.DisplayHome,
                CategoryUrl=addCategoryDto.CategoryUrl,
                PageTitle=addCategoryDto.Title,
                Description=addCategoryDto.Description,
                MetaDesc=addCategoryDto.MetaDesc,
                DisplayOrder=addCategoryDto.DisplayOrder,
            };
            await _repository.AddAsync(newCategory);
            newCategory.Staffs.Add(Stf);
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync()
        {
            IEnumerable<Category> categories = await _repository.GetAllAsync();
            return categories.Select(cat=>new CategoryResponseDto
            {
                Id=cat.Id,
                CategoryName=cat.Name,
                isDisplayHome=cat.DisplayHome,
                catImageUrl=cat.ImageUrl,
                catPageTitle=cat.PageTitle,
                catDescription=cat.Description,
                catMetaDescription=cat.MetaDesc,
                CatUrl=cat.CategoryUrl
            }).ToList();
        }
    }
}
