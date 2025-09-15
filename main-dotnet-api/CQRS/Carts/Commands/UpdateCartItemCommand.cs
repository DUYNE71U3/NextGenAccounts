using main_dotnet_api.DTOs;
using MediatR;

namespace main_dotnet_api.CQRS.Carts.Commands
{
    public record UpdateCartItemCommand(string UserId, UpdateCartItemDto CartItemDto) : IRequest<CartDto>;
}
