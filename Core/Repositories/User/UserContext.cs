using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories.User;

public class UserContext : DbContext
{
    [UsedImplicitly] 
    public DbSet<UserEntity> Users { get; } = null!;

    public UserContext(DbContextOptions<UserContext> options)
        : base(options)
    { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}