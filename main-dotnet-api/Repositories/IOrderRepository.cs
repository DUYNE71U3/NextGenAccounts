using main_dotnet_api.Models;

namespace main_dotnet_api.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetOrderWithItemsAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task<Order?> GetOrderByNumberAsync(string orderNumber);
        Task<Order> CreateOrderFromBasketAsync(string userId);
        Task UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<string> GenerateOrderNumberAsync();
    }
}
