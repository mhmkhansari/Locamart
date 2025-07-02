using CSharpFunctionalExtensions;
using Locamart.Application.Contracts.UseCases.Product.AddProduct;
using Locamart.Domain.Entities.Product;
using Locamart.Domain.Entities.Product.Abstracts;
using Locamart.Domain.Entities.Product.RequestModels;
using Locamart.Shared;
using Locamart.Shared.Abstracts;
using Locamart.Shared.Infrastructure;
using Mapster;

namespace Locamart.Application.UseCases.Product.AddProduct;

public class AddProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork) : ICommandHandler<AddProductCommand, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        
        var createProductRequest = request.Adapt<AddProductRequest>();

        var product = ProductEntity.Create(createProductRequest);

        await productRepository.AddAsync(product.Value, cancellationToken);

        await unitOfWork.CommitAsync(cancellationToken);

        return UnitResult.Success<Error>();
    }
}

