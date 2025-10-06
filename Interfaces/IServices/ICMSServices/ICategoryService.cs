using ECommerce.DTOs.CMSDTOs;

namespace ECommerce.Interfaces.IServices.ICMSServices
{
    public interface ICategoryService
    {
        public Task AddCategoryAsync(AddCategoryDto addCategoryDto);
        public Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync();
    }
}
