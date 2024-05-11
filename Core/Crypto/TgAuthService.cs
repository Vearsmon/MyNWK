using System.Security.Cryptography;
using System.Text;

namespace Core.Crypto;

public class TgAuthService : ITgAuthService
{
    public bool IsUserMetaValid(string expectedHash, Dictionary<string, string> data)
    {
        var key = Convert.FromHexString("8b190b55c68d919b918b7ac976cf00db4504965c818a12ea35f6ec93d09a21e8");
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