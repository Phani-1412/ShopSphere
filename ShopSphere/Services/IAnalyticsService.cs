using ShopSphere.DTO;

namespace ShopSphere.Services
{
    public interface IAnalyticsService
    {
        Task<AnalyticsResponseDto> GetPlatformAnalyticsAsync();
        Task<decimal> GetSellerRevenueAsync(int sellerId);
    }
}
