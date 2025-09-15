using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_dotnet_api.Models
{
    public class Cart
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        // Computed properties
        [NotMapped]
        public decimal TotalAmount => CartItems?.Sum(x => x.TotalPrice) ?? 0;

        [NotMapped]
        public int TotalItems => CartItems?.Sum(x => x.Quantity) ?? 0;
    }
}
