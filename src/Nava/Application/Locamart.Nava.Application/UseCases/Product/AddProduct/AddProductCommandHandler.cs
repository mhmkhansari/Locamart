using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Dina.Infrastructure;
using Locamart.Nava.Application.Contracts.UseCases.Product.AddProduct;
using Locamart.Nava.Domain.Entities.Product;
using Locamart.Nava.Domain.Entities.Product.Abstracts;
using Locamart.Nava.Domain.Entities.Product.RequestModels;
using Mapster;

namespace Locamart.Nava.Application.UseCases.Product.AddProduct;

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

