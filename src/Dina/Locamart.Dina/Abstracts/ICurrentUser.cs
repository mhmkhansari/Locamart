using Locamart.Dina.ValueObjects;

namespace Locamart.Dina.Abstracts;

public interface ICurrentUser
{
    public UserId UserId { get; }
}
