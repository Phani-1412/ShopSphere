namespace ShopSphere.DTO
{
    public class CreatePaymentDto
    {
        public int OrderID { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }

    }
}
