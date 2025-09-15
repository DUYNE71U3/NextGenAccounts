using Microsoft.EntityFrameworkCore;
using main_dotnet_api.Data;
using main_dotnet_api.Models;
using System.Linq.Expressions;

namespace main_dotnet_api.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<Order?> GetOrderWithItemsAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByNumberAsync(string orderNumber)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
        }

        public async Task<Order> CreateOrderFromBasketAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.IsActive);

            if (cart == null || !cart.CartItems.Any())
                throw new InvalidOperationException("Cart is empty or not found");

            var orderNumber = await GenerateOrderNumberAsync();

            var order = new Order
            {
                UserId = userId,
                OrderNumber = orderNumber,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                OrderItems = new List<OrderItem>()
            };

            decimal subTotal = 0;
            foreach (var cartItem in cart.CartItems)
            {
                var orderItem = new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    ProductName = cartItem.Product.Name,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.UnitPrice,
                    TotalPrice = cartItem.Quantity * cartItem.UnitPrice
                };

                order.OrderItems.Add(orderItem);
                subTotal += orderItem.TotalPrice;
            }

            order.SubTotal = subTotal;
            order.TotalAmount = subTotal; // Can add tax/discount calculation here

            _context.Orders.Add(order);

            // Clear the cart after creating order
            _context.CartItems.RemoveRange(cart.CartItems);

            await _context.SaveChangesAsync();
            return order;
        }

        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await GetByIdAsync(orderId);
            if (order == null)
                throw new ArgumentException("Order not found");

            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;

            if (status == OrderStatus.Completed)
                order.CompletedAt = DateTime.UtcNow;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GenerateOrderNumberAsync()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var random = new Random().Next(1000, 9999);
            var orderNumber = $"ORD{timestamp}{random}";

            // Ensure uniqueness
            while (await _context.Orders.AnyAsync(o => o.OrderNumber == orderNumber))
            {
                random = new Random().Next(1000, 9999);
                orderNumber = $"ORD{timestamp}{random}";
            }

            return orderNumber;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> FindAsync(Expression<Func<Order, bool>> predicate)
        {
            return await _context.Orders
                .Where(predicate)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .ToListAsync();
        }

        public async Task<Order> AddAsync(Order entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            _context.Orders.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Order> UpdateAsync(Order entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Orders.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var order = await GetByIdAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Orders.AnyAsync(o => o.Id == id);
        }
    }
}