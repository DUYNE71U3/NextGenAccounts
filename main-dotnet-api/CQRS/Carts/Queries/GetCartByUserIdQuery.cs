using main_dotnet_api.DTOs;
using MediatR;

namespace main_dotnet_api.CQRS.Carts.Queries
{
    public record GetCartByUserIdQuery(string UserId) : IRequest<CartDto?>;
}
