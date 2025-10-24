
namespace Locamart.Dina.Utils;

public static class DinaGuid
{
    public static Guid NewSequentialGuid()
    {
        var guidArray = Guid.NewGuid().ToByteArray();
        var timestamp = BitConverter.GetBytes(DateTime.UtcNow.Ticks);

        Array.Copy(timestamp, timestamp.Length - 6, guidArray, guidArray.Length - 6, 6);

        return new Guid(guidArray);
    }
}

