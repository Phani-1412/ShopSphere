namespace ShopSphere.DTO
{
    public class OrderResponseDto
    {
        public int OrderID { get; set; }
        public decimal TotalAmount { get; set; } 
        public string CustomerName { get; set; }
        public string Status { get; set; }
    }
}
