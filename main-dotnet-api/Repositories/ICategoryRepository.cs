using main_dotnet_api.Models;

namespace main_dotnet_api.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category?> GetByIdWithProductsAsync(int id);
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
        Task<bool> IsNameUniqueAsync(string name, int? excludeId = null);
    }
}