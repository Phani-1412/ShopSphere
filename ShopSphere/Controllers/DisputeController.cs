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
    public class DisputeController : ControllerBase
    {
        private readonly IDisputeService _service;
        private readonly ApplicationDbContext _context;
        public DisputeController(IDisputeService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        // /api/dispute
        public async Task<IActionResult> Raise(CreateDisputeDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _service.RaiseDisputeAsync(userId, dto);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("resolve/{disputeId}")]
        // /api/dispute/resolve/{disputeId}
        public async Task<IActionResult> Resolve(int disputeId, ResolveDisputeDto dto)
        {
            var adminUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _service.ResolveDisputeAsync(disputeId, dto.ResolutionNote, adminUserId);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        // /api/dispute
        public async Task<IActionResult> GetAllDisputes()
        {
            var disputes = await _context.Disputes
                .Select(d => new { d.DisputeID, d.OrderID, d.Reason, d.Status, d.ResolutionNote, d.CreatedDate })
                .ToListAsync();
            return Ok(disputes);
        }
    }
}
