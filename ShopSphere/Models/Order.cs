using System.ComponentModel.DataAnnotations;

namespace ShopSphere.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        public int CustomerID { get; set; }
        public User Customer {  get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Placed";
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public Shipment Shipment { get; set; }
        public ICollection<ReturnRequest> ReturnRequests { get; set; } = new List<ReturnRequest>();
        public ICollection<Dispute> Disputes { get; set; } = new List<Dispute>();

    }
}
