using Locamart.Dina.ValueObjects;

namespace Locamart.Dina.Abstracts;

public interface IAuditable
{
    void SetCreated(DateTime now, UserId user);
    void SetUpdated(DateTime now, UserId? user);
}
