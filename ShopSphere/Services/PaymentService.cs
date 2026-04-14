using Microsoft.EntityFrameworkCore;
using ShopSphere.Data;
using ShopSphere.DTO;
using ShopSphere.Models;

namespace ShopSphere.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<string> CreatePaymentAsync(int customerId, CreatePaymentDto dto)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderID == dto.OrderID && o.CustomerID == customerId);

            if (order == null)
                return "Order not found.";

            if (order.Status != "Placed")
                return "Payment already processed or invalid order state.";

            if (order.TotalAmount != dto.Amount)
                return "Payment amount mismatch.";

            var payment = new PaymentReference
            {
                OrderID = dto.OrderID,
                Amount = dto.Amount,
                Method = dto.Method
            };

            _context.PaymentReferences.Add(payment);

            order.Status = "Paid";

            await _context.Notifications.AddAsync(new Notification
            {
                UserID = customerId,
                Message = $"Payment of ₹{dto.Amount} for Order #{dto.OrderID} was successful!",
                Category = "Payment",
                Status = "Unread",
                CreatedDate = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            return "Payment successful. Order marked as Paid.";
        }

    }
}
