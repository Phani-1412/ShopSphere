using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopSphere.Data;
using ShopSphere.DTO;
using ShopSphere.Services;
using System.Security.Claims;

namespace ShopSphere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly ApplicationDbContext _context;

        public OrderController(IOrderService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("create")]
        // /api/order/create
        public async Task<IActionResult> Create(CreateOrderDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _service.CreateOrderAsync(userId, dto);

            return Ok(result);
        }

        [Authorize(Roles = "Seller")]
        [HttpGet("seller-orders")]
        // /api/order/seller-orders
        public async Task<IActionResult> GetSellerOrders()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var seller = await _context.Sellers
                .FirstOrDefaultAsync(s => s.UserID == userId);

            if (seller == null)
                return BadRequest("Seller not found.");

            var orders = await _service.GetSellerOrdersAsync(seller.SellerID);

            return Ok(orders);
        }

        [Authorize(Roles = "Seller")]
        [HttpPut("{orderId}")]
        // /api/order/{orderId}
        public async Task<IActionResult> UpdateStatus(int orderId, UpdateOrderStatusDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var seller = await _context.Sellers
                .FirstOrDefaultAsync(s => s.UserID == userId);

            if (seller == null)
                return BadRequest("Seller not found.");

            var result = await _service.UpdateOrderStatusAsync(
                seller.SellerID,
                orderId,
                dto.Status
            );

            return Ok(result);
        }
        [Authorize]
        [HttpGet("{orderId}")]
        // /api/order/{orderId}
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            var result = await _service.GetOrderDetailsAsync(orderId);
            return Ok(result);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        // /api/order/all
        public async Task<IActionResult> GetAllOrders()
        {
            var orders=await _context.Orders
                .Include(o=>o.Customer)
                .Select(o=> new OrderResponseDto
                {
                    OrderID=o.OrderID,
                    TotalAmount=o.TotalAmount,
                    CustomerName=o.Customer.Name,
                    Status =o.Status
            })
                .ToListAsync();
            return Ok(orders);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("cart/checkout")]
        public async Task<IActionResult> Checkout()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            await _service.CheckoutAsync(userId);

            return Ok("Checkout successful");
        }


        [Authorize(Roles = "Customer")]
        [HttpDelete("cart/remove/{productId}")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            await _service.RemoveFromCartAsync(userId, productId);

            return Ok("Item removed");
        }


        [Authorize(Roles = "Customer")]
        [HttpGet("cart")]
        public async Task<IActionResult> GetCart()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var cart = await _service.GetCartAsync(userId);

            if (cart == null || cart.Items.Count == 0)
                return Ok(new { message = "Cart is empty" });

            return Ok(cart);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("cart/add")]
        public async Task<IActionResult> AddToCart([FromBody] CartItemDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (dto.ProductId <= 0 || dto.Quantity <= 0)
                return BadRequest("Invalid product or quantity");

            await _service.AddToCartAsync(userId, dto.ProductId, dto.Quantity);

            return Ok("Item added to cart");
        }



    }
}
