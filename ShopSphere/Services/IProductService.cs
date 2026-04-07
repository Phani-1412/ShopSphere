using ShopSphere.DTO;

namespace ShopSphere.Services
{
    public interface IProductService
    {
        Task<ProductResponseDto> CreateProductAsync(int userId, CreateProductDto dto);
        Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();

    }
}
