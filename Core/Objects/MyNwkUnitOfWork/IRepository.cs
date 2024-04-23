namespace Core.Objects.MyNwkUnitOfWork;

public interface IRepository<TEntity>
{
    void Create(TEntity entity);

    void Update(TEntity entity);

    Task<List<TExtractedEntity>> GetAsync<TExtractedEntity>(
        Func<IQueryable<TEntity>, IQueryable<TExtractedEntity>> gettingRequest,
        CancellationToken cancellationToken);

    Task<int> DeleteAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>> predicate,
        CancellationToken cancellationToken);
}