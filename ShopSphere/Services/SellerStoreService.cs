using ShopSphere.Data;
using ShopSphere.DTO;
using ShopSphere.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
namespace ShopSphere.Services
{
    public class SellerStoreService : ISellerStoreService
    {
        private readonly ApplicationDbContext _context;
        public SellerStoreService(ApplicationDbContext context)
        {
            _context = context;		
        }

        public async Task<string> CreateSellerStoreAsync(int sellerId, CreateSellerStoreAsyncDTO dto)
        {
            var newStore = new SellerStore
            {
                SellerID = sellerId,
                CategoryFocus = dto.CategoryFocus,
                Rating = 0,
                Status = "Active"
            };

            _context.SellerStores.Add(newStore);
            await _context.SaveChangesAsync();

            return "Seller store created successfully.";
        }

        public async Task<string> DeleteSellerStoreAsync(int sellerId, int storeId)
        {

            var store = await _context.SellerStores.FirstOrDefaultAsync(s => s.StoreID == storeId && s.SellerID == sellerId);

            if (storeId == 0) return "Cannot delete the primary store account.";
            _context.SellerStores.Remove(store);

            await _context.SaveChangesAsync();

            return "Store deleted successfully.";
        }

        public async Task<IEnumerable<SellerStoreListResponseDto>> GetAllSellersStoresAsync(int sellerId)
        {
            return await _context.SellerStores
                .Where(s => s.SellerID == sellerId)
                .Select(s => new SellerStoreListResponseDto
                {
                    StoreId = s.StoreID,
                    CategoryFocus = s.CategoryFocus,
                    Rating = s.Rating,
                    Status = s.Status
                })
                .ToListAsync();
        }

        public async Task<string> UpdateStoreStatusAsync(int sellerId, int storeId, UpdateStoreStatusAsyncDTO dto)
        {
            var store = await _context.SellerStores.FirstOrDefaultAsync(s => s.StoreID == storeId && s.SellerID == sellerId);

            if (storeId == 0) return "Cannot update the primary store status from this endpoint. Admins must approve it.";

            store.Status = dto.Status;

            await _context.SaveChangesAsync();

            return $"Store status successfully updated to {dto.Status}.";
        }
    }
}


