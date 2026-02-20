using Locamart.Dina.Abstracts;

namespace Locamart.Dina;

public sealed class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}

