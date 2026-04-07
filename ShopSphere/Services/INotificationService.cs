using ShopSphere.DTO;

namespace ShopSphere.Services
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(int userId, string message, string category);
        Task<IEnumerable<NotificationResponseDto>> GetUserNotificationsAsync(int userId);
        Task<string> MarkAsReadAsync(int notificationId);
    }
}