using main_dotnet_api.DTOs;
using MediatR;

namespace main_dotnet_api.CQRS.Products.Commands
{
    public record UpdateProductCommand(UpdateProductDto ProductDto) : IRequest<ProductDto>;
}
