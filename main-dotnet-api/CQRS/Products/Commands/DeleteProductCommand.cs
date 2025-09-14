using MediatR;

namespace main_dotnet_api.CQRS.Products.Commands
{
    public record DeleteProductCommand(int Id) : IRequest<bool>;
}
