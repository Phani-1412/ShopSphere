namespace ShopSphere.DTO
{
    public class NotificationResponseDto
    {
        public int NotificationID { get; set; }
        public string Message { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
