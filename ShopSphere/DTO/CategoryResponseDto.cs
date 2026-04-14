namespace ShopSphere.DTO
{
    public class CategoryResponseDto
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryID { get; set; }
        public string ParentCategoryName { get; set; }
        public List<CategoryResponseDto> SubCategories { get; set; } = new();

    }
}
