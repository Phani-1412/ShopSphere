using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopSphere.Models
{
    public class Seller
    {
        [Key]
        public int SellerID { get; set; }

        public int UserID { get; set; }

        public string StoreName { get; set; }

        public string ComplianceStatus { get; set; } = "Pending";// Pending, Approved
        public string? RejectionReason { get; set; }
        public int? ReviewedByAdminId {  get; set; }
        public User User { get; set; }
        public ICollection<SellerStore> SellerStores { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
