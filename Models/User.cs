using Microsoft.EntityFrameworkCore;

namespace MyNWK.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UserContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public UserContext(DbContextOptions<UserContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}