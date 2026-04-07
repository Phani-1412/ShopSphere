namespace ShopSphere.DTO
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public int CategoryId {  get; set; }
        public int StoreId { get; set; }   

    }
}
