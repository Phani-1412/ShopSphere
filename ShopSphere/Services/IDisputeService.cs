using ShopSphere.DTO;

namespace ShopSphere.Services
{
    public interface IDisputeService
    {
        Task<string> RaiseDisputeAsync(int userId, CreateDisputeDto dto);
        Task<string> ResolveDisputeAsync(int disputeId, string resolutionNote, int adminUserId);
    }
}
