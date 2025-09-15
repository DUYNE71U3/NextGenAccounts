using System.ComponentModel.DataAnnotations;

namespace main_dotnet_api.DTOs
{
    public class CartDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }
        public List<CartItemDto> CartItems { get; set; } = new();
    }

    public class CartItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ProductImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime AddedAt { get; set; }
        public ProductDto? Product { get; set; }
    }

    public class AddToCartDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; } = 1;
    }

    public class UpdateCartItemDto
    {
        [Required]
        public int CartItemId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }
    }

    public class RemoveFromCartDto
    {
        [Required]
        public int CartItemId { get; set; }
    }
}
