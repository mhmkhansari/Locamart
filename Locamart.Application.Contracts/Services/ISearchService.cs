using CSharpFunctionalExtensions;
using Locamart.Application.Contracts.Dtos;
using Locamart.Shared;

namespace Locamart.Application.Contracts.Services;

public interface ISearchService
{
    Task<Result<IReadOnlyCollection<ProductDto>, Error>> GetNearbyProducts(string query, long distance, double lat, double lon);
}

