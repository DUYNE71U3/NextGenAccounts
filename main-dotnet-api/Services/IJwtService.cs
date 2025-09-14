using main_dotnet_api.Models;

namespace main_dotnet_api.Services
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user, IList<string> roles);
    }
}
