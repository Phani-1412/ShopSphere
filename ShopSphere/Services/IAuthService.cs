using ShopSphere.DTO;

namespace ShopSphere.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto model);
        Task<object> LoginAsync(LoginDto model);
    }
}
