using main_dotnet_api.DTOs;
using MediatR;

namespace main_dotnet_api.CQRS.Orders.Commands
{
    public record CreateOrderCommand(string UserId, CreateOrderDto OrderDto) : IRequest<OrderDto>;
}
