using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using ShopSphere.DTO;
using ShopSphere.Services;
using ShopSphere.Data;

namespace ShopSphere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private readonly IShipmentService _service;
        private readonly ApplicationDbContext _context;

        public ShipmentController(IShipmentService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        [Authorize(Roles = "Seller")]
        [HttpPost]
        // /api/shipment
        public async Task<IActionResult> Create(CreateShipmentDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var seller = await _context.Sellers
                .FirstOrDefaultAsync(s => s.UserID == userId);

            if (seller == null)
                return BadRequest("Seller not found.");

            var result = await _service.CreateShipmentAsync(seller.SellerID, dto);

            return Ok(result);
        }

        [Authorize(Roles = "Logistics")]
        [HttpPut("update-status/{shipmentId}")]
        // /api/shipment/update-status/{shipmentId}
        public async Task<IActionResult> UpdateStatus(int shipmentId, UpdateShipmentStatusDto dto)
        {
            var result = await _service.UpdateShipmentStatusAsync( shipmentId, dto.Status);

            return Ok(result);
        }

        //create get all shipments for logistics
        [Authorize(Roles = "Logistics")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllShipments()
        {
            var shipments = await _context.Shipments
                .Select(s => new
                {
                    s.ShipmentID,
                    s.OrderID,
                    s.Carrier,
                    s.TrackingNumber,
                    s.DispatchDate,
                    s.DeliveryDate,
                    s.Status
                }).ToListAsync();
            return Ok(shipments);
        }

        [Authorize(Roles = "Logistics")]
        [HttpGet("ready-orders")]
        public async Task<IActionResult> GetReadyOrders()
        {
            var orders = await _context.Orders
                .Where(o => o.Status == "Packed" && o.Shipment == null)
                .Select(o => new { o.OrderID, o.TotalAmount, o.OrderDate }).ToListAsync();
            return Ok(orders);
        }

    }
}
