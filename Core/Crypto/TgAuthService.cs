using System.Security.Cryptography;
using System.Text;

namespace Core.Crypto;

public class TgAuthService : ITgAuthService
{
    public bool IsUserMetaValid(string expectedHash, Dictionary<string, string> data)
    {
        var key = Convert.FromHexString("");
        return ValidateData(key, expectedHash, data);
    }

    public bool ValidateData(byte[] key, string expectedHash, Dictionary<string, string> data)
    {
        var hmac = new HMACSHA256(key);
        var hashedData = hmac.ComputeHash(Encoding.UTF8.GetBytes(CombineString(data)));

        return expectedHash == Convert.ToHexString(hashedData).ToLower();
    }
    
    private static string CombineString(IReadOnlyDictionary<string, string> meta)
    {
        return string.Join(
            '\n',
            ITgAuthService.KeysToUseInHash
                .Select(key => (key, value: meta[key]))
                .Where(t => t.value != "undefined")
                .Select(t => Format(t.key, t.value))
                .ToArray());
        
        string Format(string key, string value) => $"{key}={value}";
    }
}