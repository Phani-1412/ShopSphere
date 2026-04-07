using ShopSphere.DTO;

namespace ShopSphere.Services
{
    public interface IShipmentService
    {
        Task<string> CreateShipmentAsync(int sellerId, CreateShipmentDto dto);
        Task<string> UpdateShipmentStatusAsync(int shipmentId, string status);
    }
}
