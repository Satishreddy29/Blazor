using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CustomerBlazorServerApp.Data
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Sku { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public string TrackingId { get; set; } = string.Empty;
        public string TrackingStatus { get; set; } = string.Empty;
        public int? CustomerId { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [JsonIgnore]
        public virtual Category? Category { get; set; }
    }
}
