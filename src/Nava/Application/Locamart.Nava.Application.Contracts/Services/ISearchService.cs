using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Nava.Application.Contracts.Dtos.Search;
using Locamart.Nava.Application.Contracts.IntegrationEvents;

namespace Locamart.Nava.Application.Contracts.Services;

public interface ISearchService
{
    Task<Result<IReadOnlyCollection<ProductSearchDto>, Error>> GetNearbyProducts(string query, long distance, double lat, double lon);
    Task<UnitResult<Error>> IndexProduct(ProductCreatedIntegrationEvent integrationEvent);
}

