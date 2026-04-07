using Microsoft.EntityFrameworkCore;
using ShopSphere.Data;
using ShopSphere.DTO;
using ShopSphere.Models;

namespace ShopSphere.Services
{
    public class ProductAttributeService : IProductAttributeService
    {
        private readonly ApplicationDbContext _context;

        public ProductAttributeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddAttributeAsync(int userId, CreateProductAttributeDto dto)
        {
            var seller = await _context.Sellers
                .FirstOrDefaultAsync(s => s.UserID == userId);

            if (seller == null)
                return "Seller not found.";

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductID == dto.ProductID &&
                                          p.SellerID == seller.SellerID);

            if (product == null)
                return "Product not found or not owned by seller.";

            var attribute = new ProductAttribute
            {
                ProductID = dto.ProductID,
                Name = dto.Name,
                Value = dto.Value
            };

            _context.ProductAttributes.Add(attribute);
            await _context.SaveChangesAsync();

            return "Attribute added.";
        }

        public async Task<IEnumerable<object>> GetAttributesAsync(int productId)
        {
            return await _context.ProductAttributes
                .Where(pa => pa.ProductID == productId)
                .Select(pa => new
                {
                    pa.Name,
                    pa.Value
                }).ToListAsync();
        }
    }
}
