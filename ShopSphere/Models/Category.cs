using System.ComponentModel.DataAnnotations;

namespace ShopSphere.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        public string Name { get; set; }

        public int? ParentCategoryID { get; set; }

        public Category ParentCategory { get; set; }

        public ICollection<Category> SubCategories { get; set; } = new List<Category>();

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
