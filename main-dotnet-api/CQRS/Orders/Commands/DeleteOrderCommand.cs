using MediatR;

namespace main_dotnet_api.CQRS.Orders.Commands
{
    public record DeleteOrderCommand(int Id) : IRequest<bool>;
}
