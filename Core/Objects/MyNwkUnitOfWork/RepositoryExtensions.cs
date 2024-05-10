using Core.Objects.Products;

namespace Core.Objects.MyNwkUnitOfWork;

public static class RepositoryExtensions
{
    public static Task<TEntity?> FirstOrDefaultAsync<TEntity>(this Task<List<TEntity>> task) => 
        task.ContinueWith(t => t.Result.FirstOrDefault());

    public static Task<Dictionary<TKey, TValue>> ToDictionaryAsync<TEntity, TKey, TValue>(
        this Task<List<TEntity>> task,
        Func<TEntity, TKey> keySelector,
        Func<TEntity, TValue> valueSelector) 
        where TKey : notnull =>
        task.ContinueWith(t => t.Result.ToDictionary(keySelector, valueSelector));

    public static Task<List<TResult>> GetPageAsync<TEntity, TResult>(
        this IRepository<TEntity> repository,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> orderer,
        Func<IQueryable<TEntity>, IQueryable<TResult>> selector,
        int pageNumber,
        int batchSize,
        CancellationToken cancellationToken = default) =>
        repository.GetAsync(
            r => selector(orderer(r)).Skip(pageNumber * batchSize).Take(batchSize),
            cancellationToken);

    public static IQueryable<Product> ProductOrderer(this IQueryable<Product> queryable) =>
        queryable
            .OrderBy(t => t.CreatedAt)
            .ThenBy(t => t.MarketId)
            .ThenBy(t => t.ProductId);
}