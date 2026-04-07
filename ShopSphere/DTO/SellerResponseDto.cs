namespace ShopSphere.DTO
{
    public class SellerResponseDto
    {
        public int SellerId { get; set; }
        public string StoreName {  get; set; }
        public string ComplianceStatus {  get; set; }
        public string? RejectionReason { get; set; }
        public int? ReviewedByAdminId { get; set; }
    }
}
