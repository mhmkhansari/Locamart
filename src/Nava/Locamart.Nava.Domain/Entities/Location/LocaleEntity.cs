using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Nava.Domain.Entities.Location.Enums;
using Locamart.Nava.Domain.Entities.Location.ValueObjects;


namespace Locamart.Nava.Domain.Entities.Location;

public sealed class LocaleEntity : AuditableEntity<LocaleId>
{
    public CityId CityId { get; private set; }

    public string Name { get; private set; }
    public string Slug { get; private set; }

    public LocaleStatus Status { get; private set; }

    private LocaleEntity(LocaleId id) : base(id) { }

    private LocaleEntity(
        LocaleId id,
        CityId cityId,
        string name,
        string slug
    ) : base(id)
    {
        CityId = cityId;
        Name = name;
        Slug = slug;
        Status = LocaleStatus.Active;
    }

    public static Result<LocaleEntity, Error> Create(
        CityId cityId,
        string name,
        string slug
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return Error.Create("locale_name_required", "Locale name is required");

        if (string.IsNullOrWhiteSpace(slug))
            return Error.Create("locale_slug_required", "Locale slug is required");

        return new LocaleEntity(
            id: default!,
            cityId,
            name,
            slug
        );
    }

    public void Deactivate() => Status = LocaleStatus.Inactive;
}

