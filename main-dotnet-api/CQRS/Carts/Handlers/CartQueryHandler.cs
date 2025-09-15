using AutoMapper;
using main_dotnet_api.CQRS.Carts.Queries;
using main_dotnet_api.DTOs;
using main_dotnet_api.Repositories;
using MediatR;

namespace main_dotnet_api.CQRS.Carts.Handlers
{
    public class GetCartByUserIdHandler : IRequestHandler<GetCartByUserIdQuery, CartDto?>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public GetCartByUserIdHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<CartDto?> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetCartWithItemsByUserIdAsync(request.UserId);
            return cart == null ? null : _mapper.Map<CartDto>(cart);
        }
    }

    public class GetCartItemsCountHandler : IRequestHandler<GetCartItemsCountQuery, int>
    {
        private readonly ICartRepository _cartRepository;

        public GetCartItemsCountHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<int> Handle(GetCartItemsCountQuery request, CancellationToken cancellationToken)
        {
            return await _cartRepository.GetCartItemsCountAsync(request.UserId);
        }
    }

    public class GetCartTotalHandler : IRequestHandler<GetCartTotalQuery, decimal>
    {
        private readonly ICartRepository _cartRepository;

        public GetCartTotalHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<decimal> Handle(GetCartTotalQuery request, CancellationToken cancellationToken)
        {
            return await _cartRepository.GetCartTotalAsync(request.UserId);
        }
    }
}
