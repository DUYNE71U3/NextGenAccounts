using MediatR;

namespace main_dotnet_api.CQRS.Carts.Commands
{
    public record ClearCartCommand(String UserId) : IRequest<bool>;
}
