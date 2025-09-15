using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_dotnet_api.Models
{
    public enum OrderStatus
    {
        Pending = 0,        // Đang chờ thanh toán
        Paid = 1,           // Đã thanh toán thành công
        Completed = 2,      // Đã giao tài khoản thành công
        Cancelled = 3,      // Đã hủy
        Refunded = 4        // Đã hoàn tiền
    }
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string OrderNumber { get; set; } = string.Empty;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; } = 0;

        [StringLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        // Navigation properties
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
