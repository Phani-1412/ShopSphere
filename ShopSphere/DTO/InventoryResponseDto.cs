namespace ShopSphere.DTO
{
    public class InventoryResponseDto
    {
        public int ProductID { get; set; }
        public int AvailableQuantity { get; set; }
        public int ReorderThreshold { get; set; }
    }
}
