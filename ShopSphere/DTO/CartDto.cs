namespace ShopSphere.DTO
{
    public class CartDto
    {
        public int OrderId { get; set; }
        public List<CartItemDto> Items { get; set; }
    }
}
