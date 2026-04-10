using ShopSphere.DTO;

namespace ShopSphere.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(int customerId, CreateOrderDto dto);
        Task<IEnumerable<object>> GetSellerOrdersAsync(int sellerId);
        Task<string> UpdateOrderStatusAsync(int sellerId, int orderId, string newStatus);
        Task<object> GetOrderDetailsAsync(int orderId);
        Task<CartDto> GetCartAsync(int customerId);
        Task AddToCartAsync(int customerId,int productId, int quantity);
        Task<string> RemoveFromCartAsync(int customerId, int productId);
        Task<string> CheckoutAsync(int customerId);
    }
}
