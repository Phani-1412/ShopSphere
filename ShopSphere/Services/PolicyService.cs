using Microsoft.EntityFrameworkCore;
using ShopSphere.Data;
using ShopSphere.Models;

namespace ShopSphere.Services
{
    public class PolicyService : IPolicyService
    {
        private readonly ApplicationDbContext _context;

        public PolicyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreatePolicyAsync(Policy policy)
        {
            if (string.IsNullOrWhiteSpace(policy.Title) || string.IsNullOrWhiteSpace(policy.Content))
                return "Invalid policy data.";

            await _context.Policies.AddAsync(policy);
            await _context.SaveChangesAsync();

            return "Policy created successfully.";
        }

        public async Task<IEnumerable<Policy>> GetAllPoliciesAsync()
        {
            return await _context.Policies.ToListAsync();
        }

        public async Task<Policy> GetPolicyByIdAsync(int id)
        {
            return await _context.Policies.FindAsync(id);
        }

        public async Task<string> UpdatePolicyAsync(int id, Policy policy)
        {
            var existing = await _context.Policies.FindAsync(id);

            if (existing == null)
                return "Policy not found.";

            existing.Title = policy.Title;
            existing.Content = policy.Content;

            await _context.SaveChangesAsync();

            return "Policy updated successfully.";
        }

        public async Task<string> DeletePolicyAsync(int id)
        {
            var policy = await _context.Policies.FindAsync(id);

            if (policy == null)
                return "Policy not found.";

            _context.Policies.Remove(policy);
            await _context.SaveChangesAsync();

            return "Policy deleted successfully.";
        }
    }
}
