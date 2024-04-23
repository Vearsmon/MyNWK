using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Core.Objects;

public static class DbContextExtensions
{
    public static async Task CreateOrUpdate<TEntity>(
        this DbSet<TEntity> dbSet,
        Expression<Func<TEntity, bool>> predicate,
        TEntity entity,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        var entityFound = await dbSet
            .FirstOrDefaultAsync(predicate, cancellationToken)
            .ConfigureAwait(false);
        if (entityFound is null)
        { 
            await dbSet.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            return;
        }
        
        dbSet.Update(entity);
    }
}