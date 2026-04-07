using ShopSphere.DTO;
using ShopSphere.Models;

namespace ShopSphere.Services

{

    public interface ISellerStoreService
    {

        Task<string> CreateSellerStoreAsync(int sellerId, CreateSellerStoreAsyncDTO dto);


        Task<string> UpdateStoreStatusAsync(int sellerId, int storeId, UpdateStoreStatusAsyncDTO dto);


        Task<string> DeleteSellerStoreAsync(int sellerId, int storeId);


        Task<IEnumerable<SellerStoreListResponseDto>> GetAllSellersStoresAsync(int sellerId);

    }

}