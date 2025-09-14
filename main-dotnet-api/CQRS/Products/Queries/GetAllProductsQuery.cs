using main_dotnet_api.DTOs;
using MediatR;

namespace main_dotnet_api.CQRS.Products.Queries
{
    public record GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>;
}
