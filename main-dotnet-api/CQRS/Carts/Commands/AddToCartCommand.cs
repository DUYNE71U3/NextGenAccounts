using main_dotnet_api.DTOs;
using MediatR;

namespace main_dotnet_api.CQRS.Carts.Commands
{
    public record AddToCartCommand(string UserId, AddToCartDto CartDto) : IRequest<CartDto>;
}
