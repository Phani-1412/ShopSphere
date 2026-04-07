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
    public class ReturnController : ControllerBase
    {
        private readonly IReturnService _service;
        private readonly ApplicationDbContext _context;

        public ReturnController(IReturnService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        // /api/return
        public async Task<IActionResult> CreateReturn(CreateReturnDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _service.CreateReturnRequestAsync(userId, dto);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{returnId}")]
        // /api/return/{returnId}
        public async Task<IActionResult> Process(int returnId, ApproveReturnDto dto)
        {
            var adminUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _service.ProcessReturnAsync(returnId, dto.Approve, adminUserId);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllReturns()
        {
            var returns = await _context.ReturnRequests
                .Select(r => new { r.ReturnID, r.OrderID, r.Reason, r.Status, r.RequestedDate })
                .ToListAsync();
            return Ok(returns);
        }
    }
}
