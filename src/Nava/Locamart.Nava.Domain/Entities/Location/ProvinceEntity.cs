using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Nava.Domain.Entities.Location.Enums;
using Locamart.Nava.Domain.Entities.Location.ValueObjects;

namespace Locamart.Nava.Domain.Entities.Location;

public sealed class ProvinceEntity : AuditableEntity<ProvinceId>
{
    public string Name { get; private set; }
    public string Code { get; private set; }
    public ProvinceStatus Status { get; private set; }
    private readonly List<CityEntity> _cities = new();
    public IReadOnlyCollection<CityEntity> Cities => _cities.AsReadOnly();

    private ProvinceEntity(ProvinceId id) : base(id) { }

    private ProvinceEntity(
        ProvinceId id,
        string name,
        string code
    ) : base(id)
    {
        Name = name;
        Code = code;
        Status = ProvinceStatus.Active;
    }

    public static Result<ProvinceEntity, Error> Create(
        string name,
        string code
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return Error.Create("province_name_required", "Province name is required");

        if (string.IsNullOrWhiteSpace(code))
            return Error.Create("province_code_required", "Province code is required");

        return new ProvinceEntity(
            id: default!,
            name,
            code
        );
    }

    public void Deactivate() => Status = ProvinceStatus.Inactive;
}

