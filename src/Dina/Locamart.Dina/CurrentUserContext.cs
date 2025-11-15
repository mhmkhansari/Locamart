using Locamart.Dina.ValueObjects;

namespace Locamart.Dina;

public class CurrentUserContext
{
    public UserId? UserId { get; set; }
    public string Role { get; set; }
    public Guid? StoreId { get; set; }
}

