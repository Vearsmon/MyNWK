using Core.Objects;
using Core.Objects.MyNwkUnitOfWork;
using Core.Objects.Sellers;
using Core.Objects.Users;
using Microsoft.EntityFrameworkCore;

namespace TestsCore.Repositories;

[TestFixture]
public class UnitOfWork_Test
{
    private readonly UnitOfWork unitOfWork;
    
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
            .UseSnakeCaseNamingConvention();
        var dbContext = new CoreDbContext(optionsBuilder.Options);
        unitOfWork = new UnitOfWork(dbContext);
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
        var seller = new Seller
        {
            UserId = user.Id,
            RoomId = 1
        };
        unitOfWork.SellersRepository.Create(seller);
        await unitOfWork.CommitAsync(CancellationToken.None);
        seller = (await unitOfWork.SellersRepository.GetAsync(
                t => t
                    .Where(e => e.UserId == user.Id)
                    .Include(e => e.Room),
                CancellationToken.None)
            ).FirstOrDefault();
        Console.WriteLine(user.Id);
        Console.WriteLine(user.Seller?.Room?.RoomNumber);
    }
}