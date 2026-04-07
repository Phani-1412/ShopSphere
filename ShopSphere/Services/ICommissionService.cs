using ShopSphere.Models;

namespace ShopSphere.Services
{
    public interface ICommissionService
    {
        Task<string> SetCommissionAsync(Commission commission);
        Task<Commission> GetCommissionAsync();
        Task<decimal> CalculateCommissionAsync(decimal amount);
    }
}
