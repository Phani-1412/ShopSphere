using Microsoft.EntityFrameworkCore;
using ShopSphere.Data;
using ShopSphere.DTO;

namespace ShopSphere.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly ApplicationDbContext _context;

        public AnalyticsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AnalyticsResponseDto> GetPlatformAnalyticsAsync()
        {
            var totalOrders = await _context.Orders.CountAsync();

            var totalRevenue = await _context.Orders
                .Where(o => o.Status == "Delivered")
                .SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

            var totalRefund = await _context.Refunds
                .SumAsync(r => (decimal?)r.Amount) ?? 0;

            var totalProducts = await _context.Products.CountAsync();

            return new AnalyticsResponseDto
            {
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue,
                TotalRefundAmount = totalRefund,
                TotalProducts = totalProducts
            };
        }

        public async Task<decimal> GetSellerRevenueAsync(int sellerId)
        {
            return await _context.OrderItems
                .Where(oi =>
                    _context.Products.Any(p =>
                        p.ProductID == oi.ProductID &&
                        p.SellerID == sellerId))
                .SumAsync(oi => (decimal?)oi.UnitPrice * oi.Quantity) ?? 0;
        }
    }
}
