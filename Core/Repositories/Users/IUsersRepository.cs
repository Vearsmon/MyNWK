using Domain.Objects;

namespace Core.Repositories.Users;

public interface IUsersRepository
{
    public Task<User> GetAsync(string email);

    public Task<User?> FindAsync(string email);

    public Task CreateAsync(string email, string password);
}