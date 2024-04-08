using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories.Users;

public class UserContext : MyNwkDbContextBase<UserContext>
{
    [UsedImplicitly] 
    public DbSet<UserEntity> Users { get; } = null!;

    public UserContext(DbContextOptions<UserContext> options)
        : base(options)
    {
    }
}