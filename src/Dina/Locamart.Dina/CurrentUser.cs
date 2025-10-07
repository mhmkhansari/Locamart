using Locamart.Dina.Abstracts;
using Locamart.Dina.ValueObjects;

namespace Locamart.Dina;

public class CurrentUser(CurrentUserContext context) : ICurrentUser
{
    public UserId UserId =>
        context.UserId ?? throw new UnauthorizedAccessException("User not set in CurrentUserContext");
}

