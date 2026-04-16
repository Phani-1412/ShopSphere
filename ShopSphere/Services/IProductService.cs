using Microsoft.AspNetCore.Mvc;
using ShopSphere.DTO;
using ShopSphere.Models;

namespace ShopSphere.Services
{
    public interface IProductService
    {
        Task<string> CreateProductAsync(int userId, CreateProductDto dto);
        Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();
        Task<IEnumerable<ProductResponseDto>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductResponseDto>> GetProductsBySellerAsync(int userId);
    }
}
