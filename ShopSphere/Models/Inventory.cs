using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopSphere.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryID { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }

        public int SellerID { get; set; }

        public int AvailableQuantity { get; set; }

        public int ReorderThreshold { get; set; }

        public Product Product { get; set; }

    }
}
