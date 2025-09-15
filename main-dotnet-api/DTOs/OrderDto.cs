using main_dotnet_api.Models;

namespace main_dotnet_api.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string OrderNumber { get; set; } = string.Empty;
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new();
    }

    public class OrderItemDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public ProductDto? Product { get; set; }
    }

    public class CreateOrderDto
    {
        public string? Notes { get; set; }
        public List<CreateOrderItemDto> OrderItems { get; set; } = new();
    }

    public class CreateOrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateOrderStatusDto
    {
        public OrderStatus Status { get; set; }
        public string? Notes { get; set; }
    }
}