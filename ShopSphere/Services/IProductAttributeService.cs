using ShopSphere.DTO;

namespace ShopSphere.Services
{
    public interface IProductAttributeService
    {
        Task<string> AddAttributeAsync(int userId, CreateProductAttributeDto dto);
        Task<IEnumerable<object>> GetAttributesAsync(int productId);
    }
}
