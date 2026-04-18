using ShopSphere.Data;
using ShopSphere.DTO;
using ShopSphere.Models;

namespace ShopSphere.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuditService _auditService;

        public ShipmentService(ApplicationDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        public async Task<string> CreateShipmentAsync(int sellerId, CreateShipmentDto dto)
        {
            var order = await _context.Orders.FindAsync(dto.OrderID);

            if (order == null)
                return "Order not found.";

            if (order.Status != "Packed")
                return "Order must be Packed before shipment.";

            var shipment = new Shipment
            {
                OrderID = dto.OrderID,
                Carrier = dto.Carrier,
                TrackingNumber = dto.TrackingNumber,
                DispatchDate = DateTime.UtcNow
            };

            _context.Shipments.Add(shipment);

            await _context.SaveChangesAsync();

            return "Shipment created successfully.";
        }

        public async Task<string> UpdateShipmentStatusAsync(int shipmentId, string status)
        {
            var shipment = await _context.Shipments.FindAsync(shipmentId);
            if (shipment == null)
                return "Shipment not found.";

            shipment.Status = status;

            var order = await _context.Orders.FindAsync(shipment.OrderID);

            if (status == "Shipped" && order != null)
            {
                order.Status = "Shipped";
                _context.Notifications.Add(new Notification
                {
                    UserID = order.CustomerID,
                    Message = $"Your order #{order.OrderID} has been shipped.",
                    Category = "Shipment",
                    Status = "Unread"
                });
            }
            else if (status == "Delivered" && order != null)
            {
                shipment.DeliveryDate = DateTime.UtcNow;
                order.Status = "Delivered";
                _context.Notifications.Add(new Notification
                {
                    UserID = order.CustomerID,
                    Message = $"Your order #{order.OrderID} has been delivered.",
                    Category = "Shipment",
                    Status = "Unread"
                });
            }

            await _context.SaveChangesAsync();
            return "Shipment updated.";
        }

    }
}

