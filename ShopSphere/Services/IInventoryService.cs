using ShopSphere.DTO;

namespace ShopSphere.Services
{
    public interface IInventoryService
    {
        Task<string> CreateOrUpdateInventoryAsync(int userId, CreateInventoryDto dto);
        Task<InventoryResponseDto> GetInventoryByProductAsync(int productId);
    }
}
