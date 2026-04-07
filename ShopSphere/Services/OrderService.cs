using Microsoft.EntityFrameworkCore;
using ShopSphere.Data;
using ShopSphere.DTO;
using ShopSphere.Models;

namespace ShopSphere.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuditService _auditService;

        public OrderService(ApplicationDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        public async Task<OrderResponseDto> CreateOrderAsync(int customerId, CreateOrderDto dto)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductID == dto.ProductID);

            if (product == null)
                throw new Exception("Product not found.");

            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.ProductID == dto.ProductID);

            if (inventory == null || inventory.AvailableQuantity < dto.Quantity)
                throw new Exception("Insufficient stock.");

            // Reduce stock
            inventory.AvailableQuantity -= dto.Quantity;   

            var order = new Order
            {
                CustomerID = customerId,
                TotalAmount = product.Price * dto.Quantity,
                Status = "Placed"
            };

            _context.Orders.Add(order);

            var orderItem = new OrderItem
            {
                OrderID = order.OrderID,
                ProductID = dto.ProductID,
                Quantity = dto.Quantity,
                UnitPrice = product.Price
            };

            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();

            await _auditService.LogAsync(customerId, "Placed OrderID:" + order.OrderID);

            var user =await _context.Users.FirstOrDefaultAsync(u=>u.UserID == customerId);

            return new OrderResponseDto
            {
                OrderID = order.OrderID,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                CustomerName = user?.Name
            };


        }
        public async Task<IEnumerable<object>> GetSellerOrdersAsync(int sellerId)
        {
            return await _context.OrderItems
                .Where(oi => _context.Products
                    .Any(p => p.ProductID == oi.ProductID && p.SellerID == sellerId))
                .Select(oi => new
                {
                    oi.OrderID,
                    oi.ProductID,
                    oi.Quantity,
                    oi.UnitPrice,
                    OrderStatus = _context.Orders
                        .Where(o => o.OrderID == oi.OrderID)
                        .Select(o => o.Status)
                        .FirstOrDefault()
                }).ToListAsync();
        }
        public async Task<string> UpdateOrderStatusAsync(int sellerId, int orderId, string newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
                return "Order not found.";

            // Check if seller owns this order
            var ownsOrder = await _context.OrderItems
                .AnyAsync(oi =>
                    oi.OrderID == orderId &&
                    _context.Products.Any(p => p.ProductID == oi.ProductID && p.SellerID == sellerId));

            if (!ownsOrder)
                return "You do not have permission.";
            var currentStatus = order.Status.Trim().ToLower();
            var incomingStatus = newStatus.Trim().ToLower();

            var validTransitions = new Dictionary<string, List<string>>
            {
                { "placed", new List<string> { "paid", "cancelled" } },
                {"paid",  new List<string> { "packed"}},
                { "packed", new List<string> { "shipped" } },
                { "shipped", new List<string> { "delivered" } }
            };

            if (!validTransitions.ContainsKey(currentStatus) ||
                !validTransitions[currentStatus].Contains(incomingStatus))
                return "Invalid status transition.";

            order.Status = incomingStatus;
            await _context.SaveChangesAsync();

            return "Order status updated successfully.";
        }
        public async Task<object> GetOrderDetailsAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.Shipment)
                .Include(o => o.ReturnRequests)
                .FirstOrDefaultAsync(o => o.OrderID == orderId);

            if (order == null)
                return "Order not found.";

            return new
            {
                order.OrderID,
                order.Status,
                order.TotalAmount,
                Items = order.OrderItems.Select(oi => new
                {
                    oi.Product.Name,
                    oi.Quantity,
                    oi.UnitPrice
                }),
                Shipment = order.Shipment == null ? null : new
                {
                    order.Shipment.Carrier,
                    order.Shipment.TrackingNumber,
                    order.Shipment.Status
                }
            };
        }


    }
}
