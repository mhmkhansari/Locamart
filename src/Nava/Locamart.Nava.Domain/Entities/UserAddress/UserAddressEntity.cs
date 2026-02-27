using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.ValueObjects;
using Locamart.Nava.Domain.Entities.UserAddress.ValueObjects;

namespace Locamart.Nava.Domain.Entities.UserAddress;

public sealed class UserAddressEntity : AuditableEntity<UserAddressId>
{
    public string Name { get; private set; }
    public UserId UserId { get; private set; }
    public GeoLocation GeoLocation { get; private set; }
    public int ProvinceId { get; private set; }
    public int CityId { get; private set; }
    public string? AddressText { get; private set; }
    public string? PostalCode { get; private set; }

    private UserAddressEntity(UserAddressId id) : base(id) { }

    public static Result<UserAddressEntity, Error> Create(
        UserId userId,
        string name,
        GeoLocation location,
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
        GeoLocation geoLocation,
        int provinceId,
        int cityId,
        string? addressText,
        string? postalCode
    ) : base(id)
    {
        UserId = userId;
        Name = name;
        GeoLocation = geoLocation;
        ProvinceId = provinceId;
        CityId = cityId;
        AddressText = addressText;
        PostalCode = postalCode;
    }

    public void UpdateGeoLocation(GeoLocation geoLocation)
    {
        GeoLocation = geoLocation;
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
