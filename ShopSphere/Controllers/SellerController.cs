using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ShopSphere.DTO;
using ShopSphere.Services;

namespace ShopSphere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly ISellerService _service;

        public SellerController(ISellerService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Seller")]
        [HttpPost]
        // /api/seller
        public async Task<IActionResult> CreateSeller(CreateSellerDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _service.CreateSellerAsync(userId, dto);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        // /api/seller
        public async Task<IActionResult> GetAllSellers()
        {
            var sellers = await _service.GetAllSellersAsync();
            return Ok(sellers);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("approve/{sellerId}")]
        public async Task<IActionResult> ApproveSeller(int sellerId)
        {
            var adminUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _service.ApproveSellerAsync(sellerId, adminUserId);

            if (!result)
                return NotFound("Seller not found");

            return Ok("Seller approved successfully");
        }


        [HttpPut("reject/{sellerId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectSeller(int sellerId, [FromBody] RejectSellerDto dto)
        {
            var adminUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _service.RejectSellerAsync(sellerId, adminUserID, dto.Reason);

            if (!result)
                return NotFound("Seller not found");

            return Ok("Seller rejected successfully");
        }



        [Authorize(Roles = "Seller")]
        [HttpGet("my-profile")]
        // /api/seller/my-profile
        public async Task<IActionResult> MyProfile()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var seller = await _service.GetSellerByUserIdAsync(userId);

            return Ok(seller);
        }
    }
}
