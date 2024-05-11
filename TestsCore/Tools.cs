using System.Security.Cryptography;
using Core.Crypto;
using Core.Objects;
using Core.Objects.Categories;
using Core.Objects.MyNwkUnitOfWork;
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

    [Test]
    public async Task ConfigureCategories()
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder
            .UseNpgsql("Host=rc1d-fzm7z4p51iz3qylc.mdb.yandexcloud.net;" +
                       "Port=6432;" +
                       "Database=core;" +
                       "Username=mynwk-connection;" +
                       "Password=kn8i6S9WHAqycEH;" +
                       "Ssl Mode=Require;" +
                       "Trust Server Certificate=true;")
            .UseLazyLoadingProxies()
            .UseSnakeCaseNamingConvention()
            .LogTo(Console.WriteLine);
        var dbContext = new CoreDbContext(optionsBuilder.Options);
        var unitOfWork = new UnitOfWork(dbContext);
        
        unitOfWork.CategoriesRepository.Create(new Category { Title = "Продукты питания" });
        unitOfWork.CategoriesRepository.Create(new Category { Title = "Одежда" });
        await unitOfWork.CommitAsync(CancellationToken.None);
    }
}