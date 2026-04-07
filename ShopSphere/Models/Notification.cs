using System.ComponentModel.DataAnnotations;

namespace ShopSphere.Models
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        public string Message { get; set; }

        public string Category { get; set; }  // Order, Shipment, Return, Seller

        public string Status { get; set; } = "Unread";

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
