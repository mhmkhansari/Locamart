using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.ValueObjects;

namespace Locamart.Nava.Domain.Entities.UserProfile;

/*public sealed class UserProfileEntity : AuditableEntity<UserId>
{
    public string FullName { get; private set; }
    public UserId UserId { get; private set; }
    public Image? ProfileImage { get; private set; }
    public
   

  

    private UserProfileEntity() : base() { }

    public static Result<UserProfileEntity, Error> Create(UserId userId)
    {


        return new UserProfileEntity(userId);
    }

    private UserProfileEntity(UserId id) : base(id)
    {
        UserId = id;
    }

    public void SetFullName(string fullName)
    {
        FullName = fullName;
    }

    public void SetProfileImage(Image image)
    {
        ProfileImage = image;
    }

    public void AddAddress(UserAddress address)
    {
        if (!_addresses.Any(a => a.Equals(address)))
            _addresses.Add(address);
    }

    public void RemoveAddress(UserAddress address)
    {
        _addresses.Remove(address);
        if (_currentAddress == address)
            _currentAddress = null;
    }

    /// <summary>
    /// Updates the current address based on GPS location.
    /// Chooses the nearest saved address.
    /// </summary>
    public void UpdateCurrentAddress(Location gpsLocation)
    {
        if (_addresses.Count == 0)
        {
            _currentAddress = null;
            return;
        }

        _currentAddress = _addresses
            .OrderBy(a => a.Location.DistanceTo(gpsLocation))
            .First();
    }

    public UserAddress? GetCurrentAddress() => _currentAddress;

    public void SetStatus(UserStatus status)
    {
        Status = status;
    }
}*/