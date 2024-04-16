using Microsoft.EntityFrameworkCore;

namespace Core.Repositories;

public abstract class MyNwkDbContextBase<TContext> : DbContext
    where TContext: DbContext
{
    protected MyNwkDbContextBase(DbContextOptions<TContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}