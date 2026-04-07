namespace ShopSphere.DTO
{
    public class AnalyticsResponseDto
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalRefundAmount { get; set; }
        public int TotalProducts { get; set; }
    }
}
