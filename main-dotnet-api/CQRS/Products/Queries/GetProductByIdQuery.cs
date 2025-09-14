using main_dotnet_api.DTOs;
using MediatR;

namespace main_dotnet_api.CQRS.Products.Queries
{
    public record GetProductByIdQuery(int Id) : IRequest<ProductDto?>;
}
