using MediatR;
using main_dotnet_api.DTOs;

namespace main_dotnet_api.CQRS.Categories.Commands
{
    public record UpdateCategoryCommand(UpdateCategoryDto CategoryDto) : IRequest<CategoryDto>;
}