using Domain.Objects;

namespace Core.Repositories.Users;

public interface IUsersRepository
{
    public Task<User> Get(string email);

    public Task<User?> Find(string email);
}