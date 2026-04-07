namespace ShopSphere.DTO
{
    public class CreateInventoryDto
    {
        public int ProductID { get; set; }
        public int AvailableQuantity { get; set; }
        public int ReorderThreshold { get; set; }

    }
}
