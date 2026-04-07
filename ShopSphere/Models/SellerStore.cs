using System.ComponentModel.DataAnnotations;

namespace ShopSphere.Models
{
    public class SellerStore
    {
        [Key]
        public int StoreID { get; set; }

        public int SellerID { get; set; }

        public string CategoryFocus { get; set; }

        public decimal Rating { get; set; } = 0;

        public string Status { get; set; } = "Active";
        public Seller Seller { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
