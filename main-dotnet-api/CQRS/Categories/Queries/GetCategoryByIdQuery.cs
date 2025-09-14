using MediatR;
using main_dotnet_api.DTOs;

namespace main_dotnet_api.CQRS.Categories.Queries
{
    public record GetCategoryByIdQuery(int Id) : IRequest<CategoryDto?>;
}