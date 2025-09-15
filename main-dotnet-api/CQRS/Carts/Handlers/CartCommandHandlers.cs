using AutoMapper;
using main_dotnet_api.CQRS.Carts.Commands;
using main_dotnet_api.DTOs;
using main_dotnet_api.Models;
using main_dotnet_api.Repositories;
using MediatR;

namespace main_dotnet_api.CQRS.Carts.Handlers
{
    public class AddToCartHandler : IRequestHandler<AddToCartCommand, CartDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public AddToCartHandler(ICartRepository cartRepository, IProductRepository productRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<CartDto> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            // Get or create cart
            var cart = await _cartRepository.GetActiveCartByUserIdAsync(request.UserId);
            if (cart == null)
            {
                cart = new Cart { UserId = request.UserId };
                cart = await _cartRepository.AddAsync(cart);
            }

            // Get product
            var product = await _productRepository.GetByIdAsync(request.CartDto.ProductId);
            if (product == null || !product.IsAvailable)
                throw new InvalidOperationException("Product not found or not available");

            // Check if item already exists in cart
            var existingItem = await _cartRepository.GetCartItemAsync(cart.Id, request.CartDto.ProductId);

            if (existingItem != null)
            {
                // Update quantity
                existingItem.Quantity += request.CartDto.Quantity;
                await _cartRepository.UpdateCartItemAsync(existingItem);
            }
            else
            {
                // Add new item
                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = request.CartDto.ProductId,
                    Quantity = request.CartDto.Quantity,
                    UnitPrice = product.Price
                };
                await _cartRepository.AddItemToCartAsync(cartItem);
            }

            // Return updated cart
            var updatedCart = await _cartRepository.GetCartWithItemsByUserIdAsync(request.UserId);
            return _mapper.Map<CartDto>(updatedCart);
        }
    }

    public class UpdateCartItemHandler : IRequestHandler<UpdateCartItemCommand, CartDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public UpdateCartItemHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<CartDto> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
        {
            var carts = await _cartRepository.FindAsync(c => c.UserId == request.UserId);
            var cartItem = carts.SelectMany(c => c.CartItems)
                .FirstOrDefault(ci => ci.Id == request.CartItemDto.CartItemId);

            if (cartItem == null)
                throw new InvalidOperationException("Cart item not found");

            cartItem.Quantity = request.CartItemDto.Quantity;
            await _cartRepository.UpdateCartItemAsync(cartItem);

            var updatedCart = await _cartRepository.GetCartWithItemsByUserIdAsync(request.UserId);
            return _mapper.Map<CartDto>(updatedCart);
        }
    }

    public class RemoveFromCartHandler : IRequestHandler<RemoveFromCartCommand, CartDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public RemoveFromCartHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<CartDto> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
        {
            await _cartRepository.RemoveCartItemAsync(request.CartItemId);

            var updatedCart = await _cartRepository.GetCartWithItemsByUserIdAsync(request.UserId);
            return _mapper.Map<CartDto>(updatedCart);
        }
    }

    public class ClearCartHandler : IRequestHandler<ClearCartCommand, bool>
    {
        private readonly ICartRepository _cartRepository;

        public ClearCartHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<bool> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            await _cartRepository.ClearCartAsync(request.UserId);
            return true;
        }
    }
}
