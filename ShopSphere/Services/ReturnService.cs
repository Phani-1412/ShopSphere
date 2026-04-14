using Microsoft.EntityFrameworkCore;
using ShopSphere.Data;
using ShopSphere.DTO;
using ShopSphere.Models;

namespace ShopSphere.Services
{
    public class ReturnService : IReturnService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuditService _auditService;

        public ReturnService(ApplicationDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        public async Task<string> CreateReturnRequestAsync(int customerId, CreateReturnDto dto)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderID == dto.OrderID && o.CustomerID == customerId);

            if (order == null)
                return "Order not found.";

            if (order.Status != "Delivered")
                return "Return allowed only for delivered orders.";

            var returnRequest = new ReturnRequest
            {
                OrderID = dto.OrderID,
                Reason = dto.Reason
            };

            _context.ReturnRequests.Add(returnRequest);

            order.Status = "ReturnRequested";

            await _context.SaveChangesAsync();

            await _context.Notifications.AddAsync(new Notification
            {
                UserID = customerId,
                Message = $"Return request for Order #{dto.OrderID} submitted successfully!",
                Category = "Return",
                Status = "Unread",
                CreatedDate = DateTime.UtcNow
            });

            return "Return request submitted.";

        }

        public async Task<string> ProcessReturnAsync(int returnId, bool approve, int adminUserId)
        {
            var returnRequest = await _context.ReturnRequests
                .FirstOrDefaultAsync(r => r.ReturnID == returnId);

            if (returnRequest == null)
                return "Return request not found.";

            if (returnRequest.Status != "Pending")
                return "Return already processed.";

            var order = await _context.Orders.FindAsync(returnRequest.OrderID);
            if (order == null)
                return "Order not found.";

            if (approve)
            {
                returnRequest.Status = "Approved";
                order.Status = "Returned";

                var refund = new Refund
                {
                    ReturnID = returnRequest.ReturnID,
                    Amount = order.TotalAmount,
                    Status = "Processed",
                    ProcessedDate = DateTime.UtcNow
                };

                _context.Refunds.Add(refund);
            }
            else
            {
                returnRequest.Status = "Rejected";
                order.Status = "Delivered";
            }

            await _context.SaveChangesAsync();

            await _auditService.LogAsync(adminUserId,
                approve ? $"Approved Return ID: {returnId}" : $"Rejected Return ID: {returnId}");

            return "Return processed.";
        }

    }
}
