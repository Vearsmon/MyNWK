using System.Security.Cryptography;
using Core.Crypto;

namespace TestsCore;

[TestFixture]
public class Tools
{
    [Test]
    public void GenerateRandomByteSequence()
    {
        var bytes = RandomNumberGenerator.GetBytes(128 / 8); 
        Console.WriteLine($"Salt: {Convert.ToBase64String(bytes)}");
    }

    [Test]
    public void EvaluateHash()
    {
        var hash = new HashPasswordService().EvaluateHash("112312312313");
        Console.WriteLine(hash);
    }
}