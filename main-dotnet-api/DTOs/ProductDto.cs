namespace main_dotnet_api.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? GameTitle { get; set; }
        public string? Server { get; set; }
        public string? AccountLevel { get; set; }
        public string? AccountDetails { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int CategoryId { get; set; }
        public CategoryDto? Category { get; set; }
    }

    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? GameTitle { get; set; }
        public string? Server { get; set; }
        public string? AccountLevel { get; set; }
        public string? AccountDetails { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsAvailable { get; set; } = true;
        public bool IsFeatured { get; set; } = false;
        public int CategoryId { get; set; }
    }

    public class UpdateProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? GameTitle { get; set; }
        public string? Server { get; set; }
        public string? AccountLevel { get; set; }
        public string? AccountDetails { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsFeatured { get; set; }
        public int CategoryId { get; set; }
    }
}