using ShopSphere.Models;

public interface IPolicyService
{
    Task<string> CreatePolicyAsync(Policy policy);
    Task<IEnumerable<Policy>> GetAllPoliciesAsync();
    Task<Policy> GetPolicyByIdAsync(int id);
    Task<string> UpdatePolicyAsync(int id, Policy policy);
    Task<string> DeletePolicyAsync(int id);
}
