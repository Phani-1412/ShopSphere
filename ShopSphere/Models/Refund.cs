using System.ComponentModel.DataAnnotations;

namespace ShopSphere.Models
{
    public class Refund
    {
        [Key]
        public int RefundID { get; set; }

        public int ReturnID { get; set; }

        public decimal Amount { get; set; }

        public DateTime ProcessedDate { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Completed";
        public ReturnRequest ReturnRequest { get; set; }

    }
}
