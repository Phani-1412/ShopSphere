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
                .Include(oi => oi.Product)
                .Where(oi => oi.Product.SellerID == sellerId)
                .Select(oi => new
                {
                    orderID = oi.OrderID,
                    productID = oi.ProductID,
                    productName = oi.Product.Name,
                    quantity = oi.Quantity,
                    unitPrice = oi.UnitPrice > 0 ? oi.UnitPrice : oi.Product.Price, // Fallback if UnitPrice is 0
                    totalAmount = (oi.UnitPrice > 0 ? oi.UnitPrice : oi.Product.Price) * oi.Quantity,
                    orderStatus = _context.Orders
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

        public async Task<CartDto> GetCartAsync(int customerId)
        {
            var cartOrder = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.CustomerID == customerId && o.Status == "Cart");

            if (cartOrder == null)
                return new CartDto { Items = new List<CartItemDto>() };

            return new CartDto
            {
                OrderId = cartOrder.OrderID,
                Items = cartOrder.OrderItems.Select(oi => new CartItemDto
                {
                    ProductId = oi.ProductID,
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    Price = oi.Product.Price
                }).ToList()
            };
        }


        public async Task AddToCartAsync(int customerId, int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new Exception("Product not found");

            var cartOrder = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.CustomerID == customerId && o.Status == "Cart");

            if (cartOrder == null)
            {
                cartOrder = new Order
                {
                    CustomerID = customerId,
                    Status = "Cart",
                    TotalAmount = 0,
                    OrderItems = new List<OrderItem>()
                };

                _context.Orders.Add(cartOrder);
            }

            var existingItem = cartOrder.OrderItems
                .FirstOrDefault(i => i.ProductID == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cartOrder.OrderItems.Add(new OrderItem
                {
                    ProductID = productId,
                    Quantity = quantity,
                    UnitPrice = product.Price
                });
            }
            cartOrder.TotalAmount = cartOrder.OrderItems
                .Sum(i => i.Quantity * product.Price);

            await _context.SaveChangesAsync();
        }

        public async Task<string> RemoveFromCartAsync(int customerId, int productId)
        {
            var cartOrder = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.CustomerID == customerId && o.Status == "Cart");

            if (cartOrder == null) 
                return "Cart not found";

            var item = cartOrder.OrderItems
                .FirstOrDefault(i => i.ProductID == productId);

            if (item != null)
            {
                cartOrder.OrderItems.Remove(item);
                cartOrder.TotalAmount = cartOrder.OrderItems
                    .Sum(i => i.Quantity * i.Product.Price);

                await _context.SaveChangesAsync();
                return "Item removed from cart.";
            }
            return "Item not found in cart.";
        }

        public async Task<string> CheckoutAsync(int customerId)
        {
            var cartOrder = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.CustomerID == customerId && o.Status == "Cart");

            if (cartOrder == null || !cartOrder.OrderItems.Any())
                throw new Exception("Cart is empty");

            cartOrder.Status = "Placed";
            cartOrder.OrderDate = DateTime.UtcNow;

            await _context.Notifications.AddAsync(new Notification
            {
                UserID = customerId,
                Message = $"Your order #{cartOrder.OrderID} has been placed successfully!",
                Category = "Order",
                Status = "Unread",
                CreatedDate = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return "Checkout successful. Order ID: " + cartOrder.OrderID;
        }


    }
}
