using AutoMapper;
using MediatR;
using main_dotnet_api.CQRS.Orders.Queries;
using main_dotnet_api.DTOs;
using main_dotnet_api.Repositories;

namespace main_dotnet_api.CQRS.Orders.Handlers
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public GetOrderByIdHandler(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _repository.GetOrderWithItemsAsync(request.Id);
            return order == null ? null : _mapper.Map<OrderDto>(order);
        }
    }

    public class GetOrderByNumberHandler : IRequestHandler<GetOrderByNumberQuery, OrderDto?>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public GetOrderByNumberHandler(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OrderDto?> Handle(GetOrderByNumberQuery request, CancellationToken cancellationToken)
        {
            var order = await _repository.GetOrderByNumberAsync(request.OrderNumber);
            return order == null ? null : _mapper.Map<OrderDto>(order);
        }
    }

    public class GetOrdersByUserIdHandler : IRequestHandler<GetOrdersByUserIdQuery, IEnumerable<OrderDto>>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public GetOrdersByUserIdHandler(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _repository.GetOrdersByUserIdAsync(request.UserId);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }
    }

    public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<OrderDto>>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public GetAllOrdersHandler(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }
    }
}