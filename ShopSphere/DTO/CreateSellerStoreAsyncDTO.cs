namespace ShopSphere.DTO
{
    public class CreateSellerStoreAsyncDTO
    {
        public string CategoryFocus { get; set; }

        public decimal Rating { get; set; } = 0;

        public string Status { get; set; } = "Active";
    }
}
