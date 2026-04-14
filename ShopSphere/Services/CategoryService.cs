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

        public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync()
        {
            return await _context.Categories
                .Select(c => new CategoryResponseDto
                {
                    CategoryID = c.CategoryID,
                    Name = c.Name,
                    ParentCategoryID = c.ParentCategoryID,
                    ParentCategoryName = c.ParentCategory != null ? c.ParentCategory.Name : null,
                    SubCategories = c.SubCategories.Select(sc => new CategoryResponseDto
                    {
                        CategoryID = sc.CategoryID,
                        Name = sc.Name,
                        ParentCategoryID = sc.ParentCategoryID,
                        ParentCategoryName = sc.ParentCategory != null ? sc.ParentCategory.Name : null
                    }).ToList()
                })
                .ToListAsync();
        }

    }
}
