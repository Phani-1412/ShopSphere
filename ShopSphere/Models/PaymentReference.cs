using System.ComponentModel.DataAnnotations;

namespace ShopSphere.Models
{
    public class PaymentReference
    {
        [Key]
        public int PaymentRefID { get; set; }

        public int OrderID { get; set; }
        public Order Order { get; set; }

        public decimal Amount { get; set; }

        public string Method { get; set; }  // UPI, Card, etc.

        public string Status { get; set; } = "Completed";

    }
}
