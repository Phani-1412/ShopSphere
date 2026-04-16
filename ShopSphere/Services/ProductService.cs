using ShopSphere.Data;
using ShopSphere.DTO;
using ShopSphere.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace ShopSphere.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProductResponseDto>> GetProductsBySellerAsync(int userId)
        {
            return await _context.Products
                .Where(p => p.Seller.UserID == userId)
                .Select(p => new ProductResponseDto
                {
                    ProductID = p.ProductID,
                    Name = p.Name,
                    Price = p.Price,
                    SKU = p.SKU,
                    StoreID = p.StoreID
                }).ToListAsync();
        }

        public async Task<string> CreateProductAsync(int userId, CreateProductDto dto)
        {
            // 1. Find the Seller
            var seller = await _context.Sellers
                .FirstOrDefaultAsync(s => s.UserID == userId);

            if (seller == null) throw new Exception("Seller profile not found.");

            // 2. Find the Seller's primary/first store
            var baseStore = await _context.SellerStores
                .FirstOrDefaultAsync(st => st.SellerID == seller.SellerID);

            if (baseStore == null) throw new Exception("No store found for this seller.");

            // 3. Create the product using the baseStore.StoreID
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                SKU = dto.SKU,
                CategoryID = dto.CategoryId,
                SellerID = seller.SellerID,
                StoreID = baseStore.StoreID,
                Status = "Active"
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return "Product added successfully!";
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Store)
                .Where(p => p.Status == "Active" && p.Store.Status == "Active")
                .Select(p => new ProductResponseDto
                {
                    ProductID = p.ProductID,
                    Name = p.Name,
                    Price = p.Price,
                    SKU = p.SKU,
                    StoreID = p.StoreID
                }).ToListAsync();
        }

        public async Task<IEnumerable<ProductResponseDto>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryID == categoryId && p.Status == "Active")
                .Select(p => new ProductResponseDto
                {
                    ProductID = p.ProductID,
                    Name = p.Name,
                    Price = p.Price,
                    SKU = p.SKU,
                    StoreID = p.StoreID
                }).ToListAsync();
        }
    }
}