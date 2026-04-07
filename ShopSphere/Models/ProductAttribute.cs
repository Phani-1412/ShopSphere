using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopSphere.Models
{
    public class ProductAttribute
    {
        [Key]
        public int AttributeID { get; set; }

        public int ProductID { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public Product Product { get; set; }
    }
}
