using ShopSphere.DTO;

namespace ShopSphere.Services
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentAsync(int customerId, CreatePaymentDto dto);
    }
}
