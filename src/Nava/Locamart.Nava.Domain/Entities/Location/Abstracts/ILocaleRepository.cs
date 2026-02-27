using Locamart.Nava.Domain.Entities.Location.ValueObjects;

namespace Locamart.Nava.Domain.Entities.Location.Abstracts;

public interface ILocaleRepository
{
    Task Add(LocaleEntity locale, CancellationToken ct);

    Task<LocaleEntity?> GetById(
        LocaleId localeId,
        CancellationToken ct
    );

    Task Update(LocaleEntity locale, CancellationToken ct);
}

