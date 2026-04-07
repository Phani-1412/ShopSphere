using System.ComponentModel.DataAnnotations;
namespace ShopSphere.DTO
{
    public class UpdateStoreStatusAsyncDTO
    {
        [Required]
        public string Status { get; set; }
    }
}
