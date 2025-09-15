using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_dotnet_api.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int CartId { get; set; }

        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Cart Cart { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;

        // Computed property
        [NotMapped]
        public decimal TotalPrice => UnitPrice * Quantity;
    }
}
