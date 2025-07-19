namespace Locamart.Dina.Utils;
using System.Security.Cryptography;

public static class DinaRandomNumber
{
    public static string GenerateChallengeId()
    {
        byte[] bytes = new byte[32];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');
    }
}

