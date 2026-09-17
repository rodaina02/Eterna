using System.Security.Cryptography;
using System.Text;

namespace Eterna.Application.Contacts;

public static class IpHasher
{
    public static string? Hash(string? ipAddress, string? secret)
    {
        if (string.IsNullOrWhiteSpace(ipAddress) || string.IsNullOrWhiteSpace(secret))
        {
            return null;
        }

        var key = Encoding.UTF8.GetBytes(secret);
        var payload = Encoding.UTF8.GetBytes(ipAddress.Trim());
        var hash = HMACSHA256.HashData(key, payload);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
