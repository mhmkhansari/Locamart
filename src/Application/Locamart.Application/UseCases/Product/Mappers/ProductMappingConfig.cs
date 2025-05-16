using Locamart.Application.Contracts.UseCases.Product;
using Locamart.Domain.Product.RequestModels;
using Locamart.Domain.Product.ValueObjects;
using Mapster;

namespace Locamart.Application.UseCases.Product.Mappers;

public class ProductMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddProductCommand, AddProductRequest>()
              .Map(dest => dest.Price, src => Price.Create(src.Price, "IRR"));
    }
}
