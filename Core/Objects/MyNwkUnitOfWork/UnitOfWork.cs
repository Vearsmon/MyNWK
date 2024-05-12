using System.Runtime.Serialization;
using Core.Objects.Categories;
using Core.Objects.Markets;
using Core.Objects.Orders;
using Core.Objects.Products;
using Core.Objects.Users;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.Objects.MyNwkUnitOfWork;

public sealed class UnitOfWork : IUnitOfWork
{
    private const int RetriesCount = 3;
    private readonly CoreDbContext coreDbContext;
    
    private readonly Lazy<IRepository<User>> usersRepository;
    private readonly Lazy<IRepository<Product>> productRepository;
    private readonly Lazy<IRepository<Market>> marketsRepository;
    private readonly Lazy<IRepository<MarketInfo>> marketInfosRepository;
    private readonly Lazy<IRepository<Category>> categoriesInfosRepository;
    private readonly Lazy<IRepository<Order>> ordersRepository;

    private IDbContextTransaction? transaction;
    
    public IRepository<User> UsersRepository => usersRepository.Value;
    public IRepository<Product> ProductRepository => productRepository.Value;
    public IRepository<Market> MarketsRepository => marketsRepository.Value;
    public IRepository<MarketInfo> MarketInfosRepository => marketInfosRepository.Value;
    public IRepository<Category> CategoriesRepository => categoriesInfosRepository.Value;
    public IRepository<Order> OrdersRepository => ordersRepository.Value;

    public UnitOfWork(CoreDbContext coreDbContext)
    {
        this.coreDbContext = coreDbContext;
        ordersRepository = new Lazy<IRepository<Order>>(() => BeginTransactionIfNotExists(new Repository<Order>(coreDbContext)));
        usersRepository = new Lazy<IRepository<User>>(() => BeginTransactionIfNotExists(new Repository<User>(coreDbContext)));
        productRepository = new Lazy<IRepository<Product>>(() => BeginTransactionIfNotExists(new Repository<Product>(coreDbContext)));
        marketsRepository = new Lazy<IRepository<Market>>(() => BeginTransactionIfNotExists(new Repository<Market>(coreDbContext)));
        marketInfosRepository = new Lazy<IRepository<MarketInfo>>(() => BeginTransactionIfNotExists(new Repository<MarketInfo>(coreDbContext)));
        categoriesInfosRepository = new Lazy<IRepository<Category>>(() => BeginTransactionIfNotExists(new Repository<Category>(coreDbContext)));
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        if (transaction is null)
        {
            return;
        }
        
        var retryNumber = 0;
        while(true)
        {
            try
            {
                await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
                return;
            }
            catch (SerializationException serializationException)
            {
                Console.WriteLine($"Serialization exception was thrown. " +
                                  $"Retry: {++retryNumber}/{RetriesCount}. \n" +
                                  $"{serializationException}");
                
                if (retryNumber >= RetriesCount)
                {
                    throw;
                }
            }
        }
    }

    /// <inheritdoc/>
    /// This method disposes unit of work.
    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        if (transaction is not null)
        {
            await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
        }
        
        await coreDbContext.DisposeAsync().ConfigureAwait(false);
    }
    
    public async ValueTask DisposeAsync()
    {
        await CommitAsync(CancellationToken.None).ConfigureAwait(false);
        await coreDbContext.DisposeAsync().ConfigureAwait(false);
    }

    private T BeginTransactionIfNotExists<T>(T obj)
    {
        transaction ??= coreDbContext.Database.BeginTransaction();
        return obj;
    }
}