using AutoMapper;
using MediatR;
using main_dotnet_api.CQRS.Orders.Commands;
using main_dotnet_api.DTOs;
using main_dotnet_api.Models;
using main_dotnet_api.Repositories;

namespace main_dotnet_api.CQRS.Orders.Handlers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public CreateOrderHandler(IOrderRepository orderRepository, IProductRepository productRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderNumber = await _orderRepository.GenerateOrderNumberAsync();

            var order = new Order
            {
                UserId = request.UserId,
                OrderNumber = orderNumber,
                Status = OrderStatus.Pending,
                Notes = request.OrderDto.Notes,
                CreatedAt = DateTime.UtcNow,
                OrderItems = new List<OrderItem>()
            };

            decimal subTotal = 0;
            foreach (var itemDto in request.OrderDto.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                if (product == null || !product.IsAvailable)
                    throw new ArgumentException($"Product with ID {itemDto.ProductId} is not available");

                var orderItem = new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    ProductName = product.Name,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price,
                    TotalPrice = itemDto.Quantity * product.Price
                };

                order.OrderItems.Add(orderItem);
                subTotal += orderItem.TotalPrice;
            }

            order.SubTotal = subTotal;
            order.TotalAmount = subTotal;

            var createdOrder = await _orderRepository.AddAsync(order);
            var orderWithItems = await _orderRepository.GetOrderWithItemsAsync(createdOrder.Id);

            return _mapper.Map<OrderDto>(orderWithItems);
        }
    }

    public class CreateOrderFromCartHandler : IRequestHandler<CreateOrderFromCartCommand, OrderDto>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public CreateOrderFromCartHandler(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OrderDto> Handle(CreateOrderFromCartCommand request, CancellationToken cancellationToken)
        {
            var order = await _repository.CreateOrderFromBasketAsync(request.UserId);
            if (!string.IsNullOrEmpty(request.Notes))
            {
                order.Notes = request.Notes;
                await _repository.UpdateAsync(order);
            }

            var orderWithItems = await _repository.GetOrderWithItemsAsync(order.Id);
            return _mapper.Map<OrderDto>(orderWithItems);
        }
    }

    public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand, OrderDto>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public UpdateOrderStatusHandler(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OrderDto> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            await _repository.UpdateOrderStatusAsync(request.OrderId, request.StatusDto.Status);

            if (!string.IsNullOrEmpty(request.StatusDto.Notes))
            {
                var order = await _repository.GetByIdAsync(request.OrderId);
                if (order != null)
                {
                    order.Notes = request.StatusDto.Notes;
                    await _repository.UpdateAsync(order);
                }
            }

            var updatedOrder = await _repository.GetOrderWithItemsAsync(request.OrderId);
            return _mapper.Map<OrderDto>(updatedOrder);
        }
    }

    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly IOrderRepository _repository;

        public DeleteOrderHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var exists = await _repository.ExistsAsync(request.Id);
            if (!exists) return false;

            await _repository.DeleteAsync(request.Id);
            return true;
        }
    }
}