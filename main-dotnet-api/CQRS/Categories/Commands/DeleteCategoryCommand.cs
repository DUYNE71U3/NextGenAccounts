using MediatR;

namespace main_dotnet_api.CQRS.Categories.Commands
{
    public record DeleteCategoryCommand(int Id) : IRequest<bool>;
}