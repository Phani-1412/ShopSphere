using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using ShopSphere.Services;
using ShopSphere.Data;

namespace ShopSphere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _service;
        private readonly ApplicationDbContext _context;

        public AnalyticsController(IAnalyticsService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("platform")]
        // /api/analytics/platform
        public async Task<IActionResult> GetPlatformStats()
        {
            var result = await _service.GetPlatformAnalyticsAsync();
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("orders-summary")]
        // /api/analytics/orders-summary
        public async Task<IActionResult> GetOrdersSummary()
        {
            var totalOrders = await _context.Orders.CountAsync();
            var delivered = await _context.Orders
                .Where(o => o.Status == "Delivered")
                .CountAsync();

            var returns = await _context.ReturnRequests.CountAsync();
            var disputes = await _context.Disputes.CountAsync();

            return Ok(new
            {
                TotalOrders = totalOrders,
                DeliveredOrders = delivered,
                TotalReturns = returns,
                TotalDisputes = disputes
            });
        }


        [Authorize(Roles = "Seller")]
        [HttpGet("seller-revenue")]
        // /api/analytics/seller-revenue
        public async Task<IActionResult> GetSellerRevenue()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var seller = await _context.Sellers
                .FirstOrDefaultAsync(s => s.UserID == userId);

            if (seller == null)
                return BadRequest("Seller not found.");

            var revenue = await _service.GetSellerRevenueAsync(seller.SellerID);

            return Ok(new { Revenue = revenue });
        }
    }
}
