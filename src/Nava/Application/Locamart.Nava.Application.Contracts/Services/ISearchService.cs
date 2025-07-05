using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Nava.Application.Contracts.Dtos;

namespace Locamart.Nava.Application.Contracts.Services;

public interface ISearchService
{
    Task<Result<IReadOnlyCollection<ProductDto>, Error>> GetNearbyProducts(string query, long distance, double lat, double lon);
}

