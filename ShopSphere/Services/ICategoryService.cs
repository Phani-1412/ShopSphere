using ShopSphere.DTO;
using ShopSphere.Models;

namespace ShopSphere.Services
{
    public interface ICategoryService
    {
        Task<string> CreateCategoryAsync(CreateCategoryDto dto);
        Task<IEnumerable<Category>> GetAllAsync();
    }
}
