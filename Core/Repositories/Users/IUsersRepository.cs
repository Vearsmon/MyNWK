using Domain.Objects;

namespace Core.Repositories.Users;

public interface IUsersRepository
{
    public Task<User> GetAsync(long telegramId);

    public Task<User?> FindAsync(long telegramId);

    public Task CreateAsync(long telegramId, string telegramUsername, string? name, string? phoneNumber);
}