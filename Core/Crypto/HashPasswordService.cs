using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Core.Crypto;

public class HashPasswordService : IHashPasswordService
{
    //TODO: убрать куда-нибудь соль
    private static readonly byte[] Salt = Encoding.Unicode.GetBytes("yivs6dSWKjln9Ahe83eVlw==");
    
    public string EvaluateHash(string password)
    {
        var hash = KeyDerivation.Pbkdf2(
            password, 
            Salt,
            KeyDerivationPrf.HMACSHA256,
            310000,
            32);
        return Convert.ToBase64String(hash);
    }

    public bool ComparePassword(string actualPassword, string expectedPasswordHash)
    {
        var hash = EvaluateHash(actualPassword);
        return string.Compare(hash, expectedPasswordHash, StringComparison.Ordinal) == 0;
    }
}