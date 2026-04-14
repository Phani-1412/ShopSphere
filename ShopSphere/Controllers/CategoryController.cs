using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.DTO;
using ShopSphere.Services;

namespace ShopSphere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        // /api/category
        public async Task<IActionResult> Create(CreateCategoryDto dto)
        {
            var result = await _service.CreateCategoryAsync(dto);
            return Ok(result);
        }

        [HttpGet]
        // /api/category
        public async Task<IActionResult> GetAll()
        {
            var categories = await _service.GetAllAsync(); 
            return Ok(categories);
        }
    }
}
