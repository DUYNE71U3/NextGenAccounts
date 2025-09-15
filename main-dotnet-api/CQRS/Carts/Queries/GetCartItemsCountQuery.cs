using MediatR;

namespace main_dotnet_api.CQRS.Carts.Queries
{
    public record GetCartItemsCountQuery(string UserId) : IRequest<int>;
}
