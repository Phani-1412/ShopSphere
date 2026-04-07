using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ShopSphere.DTO;
using ShopSphere.Services;

namespace ShopSphere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAttributeController : ControllerBase
    {
        private readonly IProductAttributeService _service;

        public ProductAttributeController(IProductAttributeService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Seller")]
        [HttpPost]
        // /api/productattribute
        public async Task<IActionResult> Add(CreateProductAttributeDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _service.AddAttributeAsync(userId, dto);
            return Ok(result);
        }

        [HttpGet("{productId}")]
        // /api/productattribute/{productId}
        public async Task<IActionResult> Get(int productId)
        {
            var attributes = await _service.GetAttributesAsync(productId);
            return Ok(attributes);
        }
    }
}
