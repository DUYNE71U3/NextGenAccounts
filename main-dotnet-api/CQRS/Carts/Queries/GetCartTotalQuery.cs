using MediatR;

namespace main_dotnet_api.CQRS.Carts.Queries
{
    public record GetCartTotalQuery(string UserId) : IRequest<decimal>;
}
