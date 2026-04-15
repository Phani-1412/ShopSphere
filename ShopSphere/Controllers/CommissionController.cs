using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.Models;
using ShopSphere.Services;

namespace ShopSphere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CommissionController : ControllerBase
    {
        private readonly ICommissionService _service;

        public CommissionController(ICommissionService service)
        {
            _service = service;
        }

        // POST: api/commission
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SetCommission([FromBody] Commission commission)
        {
            if (commission == null || commission.Percentage <= 0)
                return BadRequest("Invalid commission value.");

            var result = await _service.SetCommissionAsync(commission);
            return Ok(new { message = result });
        }

        // GET: api/commission
        [HttpGet]
        public async Task<IActionResult> GetCommission()
        {
            var commission = await _service.GetCommissionAsync();

            if (commission == null)
                return NotFound("Commission not set.");

            return Ok(commission);
        }
    }
}
