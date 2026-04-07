using System;
using System.ComponentModel.DataAnnotations;

namespace ShopSphere.Models
{
    public class MarketplaceReport
    {
        [Key]
        public int ReportID { get; set; }

        public string Scope { get; set; }

        public string Metrics { get; set; }

        public DateTime GeneratedDate { get; set; } = DateTime.UtcNow;
    }
}
