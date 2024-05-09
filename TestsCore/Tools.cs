using System.Security.Cryptography;
using Core.Crypto;
using Microsoft.EntityFrameworkCore;

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
        var hash = SHA256.Create().ComputeHash("7010493041:AAHm5A2XG0nNY9GAv4lKGa9Oc9aQQ3-FbqA"u8.ToArray());
        Console.WriteLine(Convert.ToHexString(hash).ToLower());
    }

    [Test]
    public void TestConnection()
    {
        var connectionString = "INSERT YOUR CONNECTION STRING HERE";
        var opB = new DbContextOptionsBuilder<DbContext>();
        opB.UseNpgsql(connectionString);
        using var c = new DbContext(opB.Options);
    }
}