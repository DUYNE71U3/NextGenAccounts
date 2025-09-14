using AutoMapper;
using main_dotnet_api.CQRS.Categories.Commands;
using main_dotnet_api.CQRS.Products.Commands;
using main_dotnet_api.DTOs;
using main_dotnet_api.Models;
using main_dotnet_api.Repositories;
using MediatR;

namespace main_dotnet_api.CQRS.Products.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        public CreateProductHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request.ProductDto);
            var CreatedProduct = await _repository.AddAsync(product);
            return _mapper.Map<ProductDto>(CreatedProduct);
        }
    }

    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        public UpdateProductHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request.ProductDto);
            var UpdatedProduct = await _repository.UpdateAsync(product);
            return _mapper.Map<ProductDto>(UpdatedProduct);
        }
    }

    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _repository;
        public DeleteProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var exists = await _repository.ExistsAsync(request.Id);
            if(!exists) return false;
            await _repository.DeleteAsync(request.Id);
            return true;
        }
    }
}
