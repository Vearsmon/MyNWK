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

    public async Task<User> Get(string email)
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

    public async Task<User?> Find(string email)
    {
        var user = await Users
            .Where(t => t.Email == email)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);

        return user is null 
            ? null 
            : Convert(user);
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