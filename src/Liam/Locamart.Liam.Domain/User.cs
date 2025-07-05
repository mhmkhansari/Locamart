using Locamart.Liam.Domain.ValueObjects;

namespace Locamart.Liam.Domain;

public class User
{
    public Guid Id { get; private set; }
    public MobileNumber MobileNumber { get; private set; }

    public User(Guid id, MobileNumber mobileNumber)
    {
        Id = id;
        MobileNumber = mobileNumber;
    }
}

