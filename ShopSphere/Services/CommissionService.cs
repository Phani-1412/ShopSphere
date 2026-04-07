using ShopSphere.Data;
using ShopSphere.Models;
using ShopSphere.Services;
using Microsoft.EntityFrameworkCore;

public class CommissionService : ICommissionService
{
    private readonly ApplicationDbContext _context;

    public CommissionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> SetCommissionAsync(Commission commission)
    {
        // Remove old commission (only one allowed)
        var existing = await _context.Commissions.FirstOrDefaultAsync();

        if (existing != null)
        {
            existing.Percentage = commission.Percentage;
        }
        else
        {
            _context.Commissions.Add(commission);
        }

        await _context.SaveChangesAsync();
        return "Commission set successfully";
    }

    public async Task<Commission> GetCommissionAsync()
    {
        return await _context.Commissions.FirstOrDefaultAsync();
    }

    public async Task<decimal> CalculateCommissionAsync(decimal amount)
    {
        var commission = await _context.Commissions.FirstOrDefaultAsync();

        if (commission == null)
            return 0;

        return (amount * commission.Percentage) / 100;
    }
}
