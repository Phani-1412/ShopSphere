using Microsoft.EntityFrameworkCore;
using ShopSphere.Data;
using ShopSphere.DTO;
using ShopSphere.Models;

namespace ShopSphere.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationDbContext _context;

        public InventoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreateOrUpdateInventoryAsync(int userId, CreateInventoryDto dto)
        {
            var seller = await _context.Sellers
                .FirstOrDefaultAsync(s => s.UserID == userId);

            if (seller == null)
                return "Seller profile not found.";

            // Verification: Ensure the Product actually belongs to this specific Seller
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductID == dto.ProductID && p.SellerID == seller.SellerID);

            if (product == null)
                return "Product not found or not owned by seller.";

            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.ProductID == dto.ProductID);

            if (inventory == null)
            {
                inventory = new Inventory
                {
                    ProductID = dto.ProductID,
                    SellerID = seller.SellerID,
                    AvailableQuantity = dto.AvailableQuantity,
                    ReorderThreshold = dto.ReorderThreshold
                };
                _context.Inventories.Add(inventory);
            }
            else
            {
                inventory.AvailableQuantity = dto.AvailableQuantity;
                inventory.ReorderThreshold = dto.ReorderThreshold;
            }

            await _context.SaveChangesAsync();
            return "Inventory updated successfully.";
        }
        public async Task<InventoryResponseDto> GetInventoryByProductSecureAsync(int userId, int productId)
        {
            return await _context.Inventories
                .Where(i => i.ProductID == productId && i.Product.Seller.UserID == userId)
                .Select(i => new InventoryResponseDto
                {
                    ProductID = i.ProductID,
                    AvailableQuantity = i.AvailableQuantity,
                    ReorderThreshold = i.ReorderThreshold
                }).FirstOrDefaultAsync();
        }

        public async Task<InventoryResponseDto> GetInventoryByProductAsync(int productId)
        {
            return await _context.Inventories
                .Where(i => i.ProductID == productId)
                .Select(i => new InventoryResponseDto
                {
                    ProductID = i.ProductID,
                    AvailableQuantity = i.AvailableQuantity,
                    ReorderThreshold = i.ReorderThreshold
                }).FirstOrDefaultAsync();
        }
    }
}