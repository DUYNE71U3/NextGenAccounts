using main_dotnet_api.DTOs;
using MediatR;

namespace main_dotnet_api.CQRS.Orders.Commands
{
    public record CreateOrderFromCartCommand(string UserId, string? Notes) : IRequest<OrderDto>;
}
