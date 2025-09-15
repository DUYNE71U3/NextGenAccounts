using main_dotnet_api.DTOs;
using MediatR;

namespace main_dotnet_api.CQRS.Orders.Queries
{
    public record GetOrdersByUserIdQuery(string UserId) : IRequest<IEnumerable<OrderDto>>;
}
