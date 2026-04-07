using Microsoft.EntityFrameworkCore;
using ShopSphere.Data;
using ShopSphere.DTO;
using ShopSphere.Models;

namespace ShopSphere.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreateCategoryAsync(CreateCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                ParentCategoryID = dto.ParentCategoryID
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return "Category created.";
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories
                .Include(c => c.SubCategories)
                .ToListAsync();
        }
    }
}
