using Locamart.Nava.Domain.Entities.Location.ValueObjects;

namespace Locamart.Nava.Domain.Entities.Location.Abstracts;

interface ICityRepository
{
    Task Add(CityEntity city, CancellationToken ct);

    Task<CityEntity?> GetById(
        CityId cityId,
        CancellationToken ct
    );

    Task Update(CityEntity city, CancellationToken ct);
}

