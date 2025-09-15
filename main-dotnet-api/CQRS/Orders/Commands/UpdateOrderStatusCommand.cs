using main_dotnet_api.DTOs;
using MediatR;

namespace main_dotnet_api.CQRS.Orders.Commands
{
    public record UpdateOrderStatusCommand(int OrderId, UpdateOrderStatusDto StatusDto) : IRequest<OrderDto>;
}
