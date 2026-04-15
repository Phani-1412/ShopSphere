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
        public async Task<IActionResult> CreateOrUpdate(CreateInventoryDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            var userId = int.Parse(userIdClaim);
            var result = await _service.CreateOrUpdateInventoryAsync(userId, dto);

            return Ok(result);
        }

        [Authorize(Roles = "Seller")]
        [HttpGet("{productId}")]
        public async Task<IActionResult> Get(int productId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            var userId = int.Parse(userIdClaim);
            var inventory = await _service.GetInventoryByProductSecureAsync(userId, productId);

            if (inventory == null)
                return NotFound("Inventory not found or you do not have permission to view it.");

            return Ok(inventory);
        }
    }
}