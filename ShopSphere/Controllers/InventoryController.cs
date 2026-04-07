using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ShopSphere.DTO;
using ShopSphere.Services;

namespace ShopSphere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _service;

        public InventoryController(IInventoryService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Seller")]
        [HttpPost]
        // /api/inventory
        public async Task<IActionResult> CreateOrUpdate(CreateInventoryDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _service.CreateOrUpdateInventoryAsync(userId, dto);

            return Ok(result);
        }

        [HttpGet("{productId}")]
        // /api/inventory/{productId}
        public async Task<IActionResult> Get(int productId)
        {
            var inventory = await _service.GetInventoryByProductAsync(productId);
            return Ok(inventory);
        }
    }
}
