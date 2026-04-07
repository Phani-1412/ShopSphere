using Microsoft.EntityFrameworkCore;
using ShopSphere.Data;
using ShopSphere.DTO;
using ShopSphere.Models;

namespace ShopSphere.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateNotificationAsync(int userId, string message, string category)
        {
            var notification = new Notification
            {
                UserID = userId,
                Message = message,
                Category = category
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<NotificationResponseDto>> GetUserNotificationsAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserID == userId)
                .OrderByDescending(n => n.CreatedDate)
                .Select(n => new NotificationResponseDto
                {
                    NotificationID = n.NotificationID,
                    Message = n.Message,
                    Category = n.Category,
                    Status = n.Status,
                    CreatedDate = n.CreatedDate
                }).ToListAsync();
        }

        public async Task<string> MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);

            if (notification == null)
                return "Notification not found.";

            notification.Status = "Read";

            await _context.SaveChangesAsync();

            return "Notification marked as read.";
        }
    }
}
