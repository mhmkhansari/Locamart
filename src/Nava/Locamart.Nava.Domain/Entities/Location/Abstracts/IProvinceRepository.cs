using Locamart.Nava.Domain.Entities.Location.ValueObjects;


namespace Locamart.Nava.Domain.Entities.Location.Abstracts;

public interface IProvinceRepository
{
    Task Add(ProvinceEntity province, CancellationToken ct);

    Task<ProvinceEntity?> GetById(
        ProvinceId orderId,
        CancellationToken ct
    );

    Task Update(ProvinceEntity order, CancellationToken ct);
}

