using System.ComponentModel.DataAnnotations;

namespace ShopSphere.Models
{
    public class Dispute
    {
        [Key]
        public int DisputeID { get; set; }

        public int OrderID { get; set; }
        public Order Order { get; set; }

        public int RaisedByUserID { get; set; }
        public User RaisedByUser { get; set; }

        public string Reason { get; set; }

        public string Status { get; set; } = "Raised";

        public string ResolutionNote { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
