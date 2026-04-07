using ShopSphere.DTO;

namespace ShopSphere.Services
{
    public interface ISellerService
    {
        Task<string> CreateSellerAsync(int userId, CreateSellerDto dto);
        Task<IEnumerable<SellerResponseDto>> GetAllSellersAsync();
        Task<bool> ApproveSellerAsync(int sellerId, int adminUserId);
        Task<SellerResponseDto> GetSellerByUserIdAsync(int userId);
        Task<bool> RejectSellerAsync(int sellerId,int adminUserID,string reason);

    }
}
