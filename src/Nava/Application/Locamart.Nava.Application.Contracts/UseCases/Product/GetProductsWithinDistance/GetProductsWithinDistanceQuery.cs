using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;

namespace Locamart.Nava.Application.Contracts.UseCases.Product.GetProductsWithinDistance;

public record GetProductsWithinDistanceQuery : IQuery<Result<GetProductsWithinDistanceQueryResult, Error>>
{
    public long Distance { get; init; }
    public string Product { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
}

