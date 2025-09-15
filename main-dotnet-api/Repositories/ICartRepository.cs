using main_dotnet_api.Models;

namespace main_dotnet_api.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart?> GetActiveCartByUserIdAsync(string userId);
        Task<Cart?> GetCartWithItemsByUserIdAsync(string userId);
        Task<CartItem?> GetCartItemAsync(int cartId, int productId);
        Task<CartItem> AddItemToCartAsync(CartItem cartItem);
        Task<CartItem> UpdateCartItemAsync(CartItem cartItem);
        Task RemoveCartItemAsync(int cartItemId);
        Task ClearCartAsync(string userId);
        Task<int> GetCartItemsCountAsync(string userId);
        Task<decimal> GetCartTotalAsync(string userId);
    }
}
