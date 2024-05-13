using Core.Objects.Orders;
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
            .OrderByDescending(t => t.CreatedAt)
            .ThenBy(t => t.MarketId)
            .ThenBy(t => t.ProductId);
    
    public static Task<Product?> GetProduct(
        this IRepository<Product> repository,
        int marketId,
        int productId,
        CancellationToken cancellationToken) =>
        repository.GetAsync(
                r => r
                    .Where(p => p.MarketId == marketId && p.ProductId == productId),
                cancellationToken)
            .FirstOrDefaultAsync();

    public static Task<List<Order>> GetOrder(
        this IRepository<Order> repository,
        Guid orderId,
        CancellationToken cancellationToken) =>
        repository.GetAsync(
            r => r.Where(t => t.OrderId == orderId),
            cancellationToken);
}