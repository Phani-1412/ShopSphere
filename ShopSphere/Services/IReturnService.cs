using ShopSphere.DTO;

namespace ShopSphere.Services
{
    public interface IReturnService
    {
        Task<string> CreateReturnRequestAsync(int customerId, CreateReturnDto dto);
        Task<string> ProcessReturnAsync(int returnId, bool approve, int adminUserId);
    }
}
