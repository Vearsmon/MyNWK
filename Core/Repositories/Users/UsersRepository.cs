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

    public async Task<User> GetAsync(string email)
    {
        var user = await Users
            .Where(t => t.Email == email)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        
        
        if (user is null)
        {
            //TODO: конкретизировать исключения
            throw new Exception();
        }

        return Convert(user);
    }

    public async Task<User?> FindAsync(string email)
    {
        Console.WriteLine(userContext.Users is null);
        var user = await userContext.Users
            .Where(t => t.Email == email)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);

        return user is null 
            ? null 
            : Convert(user);
    }

    public async Task CreateAsync(string email, string password)
    {
        Console.WriteLine(userContext is not null);
        await Users.AddAsync(new UserEntity
        {
            Email = email,
            Password = password,
        }).ConfigureAwait(false);
        await userContext.SaveChangesAsync().ConfigureAwait(false);
    }

    private static User Convert(UserEntity userEntity) =>
        new(
            userEntity.Id,
            userEntity.Email,
            userEntity.Password,
            userEntity.TelegramUsername,
            userEntity.Name,
            userEntity.PhoneNumber);
}