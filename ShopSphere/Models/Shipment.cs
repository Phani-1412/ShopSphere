using System.ComponentModel.DataAnnotations;

namespace ShopSphere.Models
{
    public class Shipment
    {
        [Key]
        public int ShipmentID { get; set; }

        public int OrderID { get; set; }

        public string Carrier { get; set; }

        public string TrackingNumber { get; set; }

        public DateTime DispatchDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public string Status { get; set; } = "Dispatched";
        public Order Order { get; set; }

    }
}
