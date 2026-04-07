namespace ShopSphere.DTO
{
    public class CreateCategoryDto
    {
        public string Name { get; set; }
        public int? ParentCategoryID { get; set; }
    }
}