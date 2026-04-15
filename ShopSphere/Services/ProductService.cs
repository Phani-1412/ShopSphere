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

        public async Task<ProductResponseDto> CreateProductAsync(int userId, CreateProductDto dto)
        {
            var seller = await _context.Sellers
                .FirstOrDefaultAsync(s => s.UserID == userId);

            if (seller == null) throw new Exception("Seller profile not found.");
            if (seller.ComplianceStatus != "Approved") throw new Exception("Profile not approved.");

            var store = await _context.SellerStores
                .FirstOrDefaultAsync(s => s.StoreID == dto.StoreId && s.SellerID == seller.SellerID);

            if (store == null) throw new Exception("Store not found or unauthorized.");

            var product = new Product
            {
                SellerID = seller.SellerID,
                StoreID = dto.StoreId,
                Name = dto.Name,
                Price = dto.Price,
                SKU = dto.SKU,
                CategoryID = dto.CategoryId,
                Status = "Active"
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ProductResponseDto
            {
                ProductID = product.ProductID,
                Name = product.Name,
                Price = product.Price,
                SKU = product.SKU,
                StoreID = product.StoreID
            };
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

        public async Task<IActionResult> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _context.Products
                .Where(p => p.CategoryID == categoryId && p.Status == "Active")
                .Select(p => new ProductResponseDto
                {
                    ProductID = p.ProductID,
                    Name = p.Name,
                    Price = p.Price,
                    SKU = p.SKU,
                    StoreID = p.StoreID
                }).ToListAsync();

            return new OkObjectResult(products);
        }
    }
}