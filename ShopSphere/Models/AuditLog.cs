using System;
using System.ComponentModel.DataAnnotations;

namespace ShopSphere.Models
{
    public class AuditLog
    {
        [Key]
        public int AuditID { get; set; }

        public int UserID { get; set; }

        public string Action { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
