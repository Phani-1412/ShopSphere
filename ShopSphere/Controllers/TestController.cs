using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShopSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [Authorize]
        [HttpGet("any-user")]
        public IActionResult AnyUser()
        {
            return Ok("You are authenticated.");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnly()
        {
            return Ok("Welcome Admin.");
        }

        [Authorize(Roles = "Seller")]
        [HttpGet("seller-only")]
        public IActionResult SellerOnly()
        {
            return Ok("Welcome Seller.");
        }
    }
}
