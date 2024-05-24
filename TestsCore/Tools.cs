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
        var hash = SHA256.Create().ComputeHash(""u8.ToArray());
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
            .UseNpgsql("")
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