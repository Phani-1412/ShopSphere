using ShopSphere.DTO;

namespace ShopSphere.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(int customerId, CreateOrderDto dto);
        Task<IEnumerable<object>> GetSellerOrdersAsync(int sellerId);
        Task<string> UpdateOrderStatusAsync(int sellerId, int orderId, string newStatus);
        Task<object> GetOrderDetailsAsync(int orderId);


    }
}
