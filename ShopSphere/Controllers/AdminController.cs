using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.Data;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public AdminController(ApplicationDbContext context) { _context = context; }

    [HttpGet("customers")]
    public async Task<IActionResult> GetCustomers() =>
        Ok(await _context.Users.Where(u => u.Role == "Customer")
            .Select(u => new { u.UserID, u.Name, u.Email, u.Phone, u.Role }).ToListAsync());

    [HttpGet("logistics")]
    public async Task<IActionResult> GetLogistics() =>
        Ok(await _context.Users.Where(u => u.Role == "Logistics")
            .Select(u => new { u.UserID, u.Name, u.Email, u.Phone, u.Role }).ToListAsync());
}
