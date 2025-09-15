using main_dotnet_api.Data;
using main_dotnet_api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace main_dotnet_api.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetByIdAsync(int id)
        {
            return await _context.Carts.FindAsync(id);
        }

        public async Task<IEnumerable<Cart>> GetAllAsync()
        {
            return await _context.Carts.Include(c => c.CartItems).ToListAsync();
        }

        public async Task<IEnumerable<Cart>> FindAsync(Expression<Func<Cart, bool>> predicate)
        {
            return await _context.Carts.Where(predicate).Include(c => c.CartItems).ToListAsync();
        }

        public async Task<Cart> AddAsync(Cart entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Carts.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Cart> UpdateAsync(Cart entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Carts.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var cart = await GetByIdAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Carts.AnyAsync(c => c.Id == id);
        }

        public async Task<Cart?> GetActiveCartByUserIdAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.IsActive);
        }

        public async Task<Cart?> GetCartWithItemsByUserIdAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.IsActive);
        }

        public async Task<CartItem?> GetCartItemAsync(int cartId, int productId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        }

        public async Task<CartItem> AddItemToCartAsync(CartItem cartItem)
        {
            cartItem.AddedAt = DateTime.UtcNow;
            cartItem.UpdatedAt = DateTime.UtcNow;
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<CartItem> UpdateCartItemAsync(CartItem cartItem)
        {
            cartItem.UpdatedAt = DateTime.UtcNow;
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task RemoveCartItemAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await GetActiveCartByUserIdAsync(userId);
            if (cart != null && cart.CartItems.Any())
            {
                _context.CartItems.RemoveRange(cart.CartItems);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetCartItemsCountAsync(string userId)
        {
            var cart = await GetActiveCartByUserIdAsync(userId);
            return cart?.TotalItems ?? 0;
        }

        public async Task<decimal> GetCartTotalAsync(string userId)
        {
            var cart = await GetActiveCartByUserIdAsync(userId);
            return cart?.TotalAmount ?? 0;
        }
    }
}
