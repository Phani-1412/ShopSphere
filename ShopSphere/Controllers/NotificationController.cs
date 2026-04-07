using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ShopSphere.Services;

namespace ShopSphere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationController(INotificationService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet]
        // /api/notification
        public async Task<IActionResult> GetMyNotifications()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var notifications = await _service.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }

        [Authorize]
        [HttpPut("{notificationId}")]
        // /api/notification/{notificationId}
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            var result = await _service.MarkAsReadAsync(notificationId);
            return Ok(result);
        }
    }
}
