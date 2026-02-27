using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Nava.Domain.Entities.Location.Enums;
using Locamart.Nava.Domain.Entities.Location.ValueObjects;

namespace Locamart.Nava.Domain.Entities.Location;

public sealed class CityEntity : AuditableEntity<CityId>
{
    public ProvinceId ProvinceId { get; private set; }

    public string Name { get; private set; }
    public string Slug { get; private set; }
    public CityStatus Status { get; private set; }
    private readonly List<LocaleEntity> _locales = new();
    public IReadOnlyCollection<LocaleEntity> Locales => _locales.AsReadOnly();
    private CityEntity(CityId id) : base(id) { }

    private CityEntity(
        CityId id,
        ProvinceId provinceId,
        string name,
        string slug
    ) : base(id)
    {
        ProvinceId = provinceId;
        Name = name;
        Slug = slug;
        Status = CityStatus.Active;
    }

    public static Result<CityEntity, Error> Create(
        ProvinceId provinceId,
        string name,
        string slug
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return Error.Create("city_name_required", "City name is required");

        if (string.IsNullOrWhiteSpace(slug))
            return Error.Create("city_slug_required", "City slug is required");

        return new CityEntity(
            id: default!,
            provinceId,
            name,
            slug
        );
    }

    public void Deactivate() => Status = CityStatus.Inactive;
}

