using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopSphere.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        public int SellerID { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string SKU { get; set; }

        public string Status { get; set; } = "Active";

        [ForeignKey("SellerID")]
        public Seller Seller { get; set; }
        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public Category Category { get; set; }
        public ICollection<ProductAttribute> Attributes { get; set; }=new List<ProductAttribute>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public Inventory Inventory { get; set; }
        public int StoreID { get; set; }
        [ForeignKey("StoreID")]
        public SellerStore Store { get; set; }
    }
}
