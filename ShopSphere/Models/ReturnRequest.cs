using System.ComponentModel.DataAnnotations;

namespace ShopSphere.Models
{
    public class ReturnRequest
    {
        [Key]
        public int ReturnID { get; set; }

        public int OrderID { get; set; }

        public string Reason { get; set; }

        public DateTime RequestedDate { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Pending";
        public Order Order { get; set; }
        public Refund Refund { get; set; }

    }
}
