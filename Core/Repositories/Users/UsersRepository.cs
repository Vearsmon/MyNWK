using Domain.Objects;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories.Users;

public class UsersRepository : IUsersRepository
{
    private readonly UserContext userContext;

    public UsersRepository(UserContext userContext)
    {
        this.userContext = userContext;
    }

    private DbSet<UserEntity> Users => userContext.Users;

    public async Task<User> GetAsync(long telegramId)
    {
        var user = await Users
            .Where(t => t.TelegramId == telegramId)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        
        
        if (user is null)
        {
            //TODO: конкретизировать исключения
            throw new Exception();
        }

        return Convert(user);
    }

    public async Task<User?> FindAsync(long telegramId)
    {
        var user = await userContext.Users
            .Where(t => t.TelegramId == telegramId)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);

        return user is null 
            ? null 
            : Convert(user);
    }

    public async Task CreateAsync(
        long telegramId,
        string telegramUsername,
        string? name = null,
        string? phoneNumber = null)
    {
        await Users.AddAsync(new UserEntity
        {
            TelegramId = telegramId,
            TelegramUsername = telegramUsername,
            Name = name,
            PhoneNumber = phoneNumber
        }).ConfigureAwait(false);
        await userContext.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<int> GetUsersCount()
    {
        return userContext.Users.Count();
    }

    private static User Convert(UserEntity userEntity) =>
        new(
            userEntity.Id,
            userEntity.TelegramId,
            userEntity.TelegramUsername,
            userEntity.Name,
            userEntity.PhoneNumber);
}