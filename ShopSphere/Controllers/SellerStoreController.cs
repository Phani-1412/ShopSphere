using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopSphere.Data;
using ShopSphere.DTO;
using ShopSphere.Services;
using System.Security.Claims;

namespace ShopSphere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerStoreController : ControllerBase
    {
        private readonly ISellerStoreService _sellerStoreService;
        private readonly ApplicationDbContext _context;

        public SellerStoreController(ApplicationDbContext context, ISellerStoreService sellerStoreService)
        {
            _context = context;
            _sellerStoreService = sellerStoreService;
        }

        // ---------------- Seller endpoints ----------------

        [Authorize(Roles = "Seller")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateSellerStore(CreateSellerStoreAsyncDTO dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var seller = await _context.Sellers.FirstOrDefaultAsync(s => s.UserID == userId);
            if (seller == null) return BadRequest("Seller not found");

            var result = await _sellerStoreService.CreateSellerStoreAsync(seller.SellerID, dto);
            return Ok(new { message = result });
        }

        [Authorize(Roles = "Seller")]
        [HttpGet("my-stores")]
        public async Task<IActionResult> GetAllMyStores()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var seller = await _context.Sellers.FirstOrDefaultAsync(s => s.UserID == userId);
            if (seller == null) return BadRequest("Seller profile not found.");

            var stores = await _sellerStoreService.GetAllSellersStoresAsync(seller.SellerID);
            return Ok(stores);
        }

        [Authorize(Roles = "Seller")]
        [HttpPut("update-status/{storeId}")]
        public async Task<IActionResult> UpdateStatus(int storeId, [FromBody] UpdateStoreStatusAsyncDTO dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _sellerStoreService.UpdateStoreStatusAsync(userId, storeId, dto);
            return Ok(new { message = result });
        }

        [Authorize(Roles = "Seller")]
        [HttpDelete("delete/{storeId}")]
        public async Task<IActionResult> DeleteStore(int storeId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _sellerStoreService.DeleteSellerStoreAsync(userId, storeId);
            return Ok(new { message = result });
        }

        // ---------------- Admin endpoints ----------------

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllStores()
        {
            var stores = await _context.SellerStores
                .Include(s => s.Seller).ThenInclude(se => se.User)
                .Select(s => new {
                    s.StoreID,
                    s.SellerID,
                    s.CategoryFocus,
                    s.Rating,
                    s.Status,
                    SellerName = s.Seller.User.Name,
                    StoreName = s.Seller.StoreName
                }).ToListAsync();
            return Ok(stores);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("approve/{storeId}")]
        public async Task<IActionResult> ApproveStore(int storeId)
        {
            var store = await _context.SellerStores.FindAsync(storeId);
            if (store == null) return NotFound("Store not found");
            store.Status = "Active";
            await _context.SaveChangesAsync();
            return Ok("Store approved");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("reject/{storeId}")]
        public async Task<IActionResult> RejectStore(int storeId)
        {
            var store = await _context.SellerStores.FindAsync(storeId);
            if (store == null) return NotFound("Store not found");
            store.Status = "Rejected";
            await _context.SaveChangesAsync();
            return Ok("Store rejected");
        }
    }
}
