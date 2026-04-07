using System.Threading.Tasks;
using ShopSphere.Data;
using ShopSphere.Models;

namespace ShopSphere.Services
{
    public class AuditService : IAuditService
    {
        private readonly ApplicationDbContext _context;

        public AuditService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogAsync(int userId, string action)
        {
            var log = new AuditLog
            {
                UserID = userId,
                Action = action
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
