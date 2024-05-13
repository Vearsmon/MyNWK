using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Core.Objects.MyNwkUnitOfWork;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity: class
{
    private readonly CoreDbContext dbContext;

    [UsedImplicitly]
    public DbSet<TEntity> Entities { get; set; }

    public Repository(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
        Entities = dbContext.Set<TEntity>();
    }

    public IQueryable<TEntity> Queryable => Entities;

    public void Create(TEntity entity)
    {
        Entities.Add(entity);
    }

    public void Update(TEntity entity)
    {
        Entities.Update(entity);
    }

    public async Task<List<TExtractedEntity>> GetAsync<TExtractedEntity>(
        Func<IQueryable<TEntity>, IQueryable<TExtractedEntity>> gettingRequest, 
        CancellationToken cancellationToken)
    {
        var entities = await gettingRequest(Entities)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return entities;
    }

    public async Task<int> DeleteAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>> predicate,
        CancellationToken cancellationToken)
    {
        var totalDeleted = await predicate(Entities)
            .ExecuteDeleteAsync(cancellationToken)
            .ConfigureAwait(false);

        return totalDeleted;
    }
}