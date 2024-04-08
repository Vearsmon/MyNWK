using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories.Users;

public class UserContext : DbContext
{
    [UsedImplicitly] 
    public DbSet<UserEntity> Users { get; } = null!;

    public UserContext(DbContextOptions<UserContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}