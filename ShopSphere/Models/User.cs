using System.ComponentModel.DataAnnotations;

namespace ShopSphere.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string Email { get; set; }

        public string Phone { get; set; }

        [Required]
        public string PasswordHash { get; set; }
        public ICollection<Order> Orders { get; set; }=new List<Order>();
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<AuditLog> AuditLogs { get; set; }
        public ICollection<Dispute> DisputesRaised { get; set; }

    }
}
