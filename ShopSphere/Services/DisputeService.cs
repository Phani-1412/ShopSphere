using Microsoft.EntityFrameworkCore;
using ShopSphere.Data;
using ShopSphere.DTO;
using ShopSphere.Models;

namespace ShopSphere.Services
{
    public class DisputeService : IDisputeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuditService _auditService;

        public DisputeService(ApplicationDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        public async Task<string> RaiseDisputeAsync(int userId, CreateDisputeDto dto)
        {
            var order = await _context.Orders.FindAsync(dto.OrderID);

            if (order == null)
                return "Order not found.";

            var dispute = new Dispute
            {
                OrderID = dto.OrderID,
                RaisedByUserID = userId,
                Reason = dto.Reason
            };

            _context.Disputes.Add(dispute);

            await _context.SaveChangesAsync();

            return "Dispute raised successfully.";
        }

        public async Task<string> ResolveDisputeAsync(int disputeId, string resolutionNote, int adminUserId)
        {
            var dispute = await _context.Disputes.FindAsync(disputeId);

            if (dispute == null)
                return "Dispute not found.";

            dispute.Status = "Resolved";
            dispute.ResolutionNote = resolutionNote;

            await _context.SaveChangesAsync();
            await _auditService.LogAsync(adminUserId, "Resolved Dispute Id:" + disputeId + "| Note" + resolutionNote);

            return "Dispute resolved.";
        }
    }
}
