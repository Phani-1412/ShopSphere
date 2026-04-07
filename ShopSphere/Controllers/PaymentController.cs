using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ShopSphere.DTO;
using ShopSphere.Services;

namespace ShopSphere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _service;

        public PaymentController(IPaymentService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        // /api/payment
        public async Task<IActionResult> Create(CreatePaymentDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _service.CreatePaymentAsync(userId, dto);

            return Ok(result);
        }
    }
}
