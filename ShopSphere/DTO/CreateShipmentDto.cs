namespace ShopSphere.DTO
{
    public class CreateShipmentDto
    {
        public int OrderID { get; set; }
        public string Carrier { get; set; }
        public string TrackingNumber { get; set; }
    }
}
