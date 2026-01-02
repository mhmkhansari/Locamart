using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.ValueObjects;
using Locamart.Nava.Domain.Entities.UserAddress.ValueObjects;

namespace Locamart.Nava.Domain.Entities.UserAddress;

public sealed class UserAddressEntity : AuditableEntity<UserAddressId>
{
    public string Name { get; private set; }
    public UserId UserId { get; private set; }
    public Location Location { get; private set; }
    public int ProvinceId { get; private set; }
    public int CityId { get; private set; }
    public string? AddressText { get; private set; }
    public string? PostalCode { get; private set; }

    private UserAddressEntity(UserAddressId id) : base(id) { }

    public static Result<UserAddressEntity, Error> Create(
        UserId userId,
        string name,
        Location location,
        int provinceId,
        int cityId,
        string? addressText,
        string? postalCode)
    {
        var idResult = UserAddressId.Create(Guid.NewGuid());
        if (idResult.IsFailure)
            return idResult.Error;

        return new UserAddressEntity(
            idResult.Value,
            userId,
            name,
            location,
            provinceId,
            cityId,
            addressText,
            postalCode
        );
    }

    private UserAddressEntity(
        UserAddressId id,
        UserId userId,
        string name,
        Location location,
        int provinceId,
        int cityId,
        string? addressText,
        string? postalCode
    ) : base(id)
    {
        UserId = userId;
        Name = name;
        Location = location;
        ProvinceId = provinceId;
        CityId = cityId;
        AddressText = addressText;
        PostalCode = postalCode;
    }

    public void UpdateLocation(Location location)
    {
        Location = location;
    }

    public void UpdateAddressDetails(
        int provinceId,
        int cityId,
        string addressText,
        string postalCode)
    {
        ProvinceId = provinceId;
        CityId = cityId;
        AddressText = addressText;
        PostalCode = postalCode;
    }

    public void Rename(string name)
    {
        Name = name;
    }
}
