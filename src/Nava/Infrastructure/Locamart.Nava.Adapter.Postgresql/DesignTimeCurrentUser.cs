using Locamart.Dina.Abstracts;
using Locamart.Dina.ValueObjects;

namespace Locamart.Nava.Adapter.Postgresql;

public class DesignTimeCurrentUser : ICurrentUser
{
    public UserId UserId { get; }

    public DesignTimeCurrentUser() => UserId = UserId.Create(Guid.NewGuid()).Value;
}