using Core.Objects;
using Core.Objects.Markets;
using Core.Objects.MyNwkUnitOfWork;
using Core.Objects.Products;
using Core.Services.Orders;
using Core.Services.Products;
using Core.Objects.Users;
using Microsoft.EntityFrameworkCore;
using Core.BlobStorage;

namespace TestsCore.Repositories;

[TestFixture]
public class UnitOfWork_Test
{
    private readonly UnitOfWork unitOfWork;
    private readonly UnitOfWorkProvider unitOfWorkProvider;
    private readonly IBlobStorageClient client;
    
    public UnitOfWork_Test()
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
            .LogTo(s => Console.WriteLine(s));
        var dbContext = new CoreDbContext(optionsBuilder.Options);
        unitOfWork = new UnitOfWork(dbContext);
        unitOfWorkProvider = new UnitOfWorkProvider(dbContext);
        client = new YdBlobStorageClient();
    }

    [Test]
    public async Task Test()
    {
        var user = new User
        {
            TelegramId = 123,
            TelegramUsername = "123"
        };
        unitOfWork.UsersRepository.Create(user);
        Console.WriteLine(user.Id);
        await unitOfWork.CommitAsync(CancellationToken.None);
        user.Name = "хуй";
        await unitOfWork.CommitAsync(CancellationToken.None);
        Console.WriteLine(user.Id);
        await unitOfWork.CommitAsync(CancellationToken.None);
        var market = new Market()
        {
            OwnerId = user.Id,
            MarketInfo = new MarketInfo(),
            Name = "123",
            Products = new List<Product>()
            {
                new()
                {
                    Title = "хуй",
                    CreatedAt = DateTime.UtcNow,
                    Price = 1,
                    Remained = 2
                    // CreatedAt = PreciseTimestampGenerator
                }
            }
        };
        unitOfWork.MarketsRepository.Create(market);
        await unitOfWork.CommitAsync(CancellationToken.None);
        
        Console.WriteLine(market.Id);
        Console.WriteLine(user.Id);
    }
    [Test]
    public async Task TestCli()
    {
        var y = new ProductService(unitOfWorkProvider, client);
        
        var x = y.GetOrderProductsAsync(new Core.RequestContext(){ UserId = 1212, CancellationToken = CancellationToken.None }, new Guid());

    }
}