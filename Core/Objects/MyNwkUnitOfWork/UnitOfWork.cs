using Core.Objects.Categories;
using Core.Objects.Markets;
using Core.Objects.Orders;
using Core.Objects.Products;
using Core.Objects.Users;

namespace Core.Objects.MyNwkUnitOfWork;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly CoreDbContext coreDbContext;
    
    private readonly Lazy<IRepository<User>> usersRepository;
    private readonly Lazy<IRepository<Product>> productRepository;
    private readonly Lazy<IRepository<Market>> marketsRepository;
    private readonly Lazy<IRepository<MarketInfo>> marketInfosRepository;
    private readonly Lazy<IRepository<Category>> categoriesInfosRepository;
    private readonly Lazy<IRepository<Order>> ordersRepository;

    public IRepository<User> UsersRepository => usersRepository.Value;
    public IRepository<Product> ProductRepository => productRepository.Value;
    public IRepository<Market> MarketsRepository => marketsRepository.Value;
    public IRepository<MarketInfo> MarketInfosRepository => marketInfosRepository.Value;
    public IRepository<Category> CategoriesRepository => categoriesInfosRepository.Value;
    public IRepository<Order> OrdersRepository => ordersRepository.Value;

    public UnitOfWork(CoreDbContext coreDbContext)
    {
        this.coreDbContext = coreDbContext;
        ordersRepository = new Lazy<IRepository<Order>>(() => new Repository<Order>(coreDbContext));
        usersRepository = new Lazy<IRepository<User>>(() => new Repository<User>(coreDbContext));
        productRepository = new Lazy<IRepository<Product>>(() => new Repository<Product>(coreDbContext));
        marketsRepository = new Lazy<IRepository<Market>>(() => new Repository<Market>(coreDbContext));
        marketInfosRepository = new Lazy<IRepository<MarketInfo>>(() => new Repository<MarketInfo>(coreDbContext));
        categoriesInfosRepository = new Lazy<IRepository<Category>>(() => new Repository<Category>(coreDbContext));
    }

    public Task CommitAsync(CancellationToken cancellationToken) => coreDbContext.SaveChangesAsync(cancellationToken); 

    public async ValueTask DisposeAsync()
    {
        await coreDbContext.SaveChangesAsync().ConfigureAwait(false);
        await coreDbContext.DisposeAsync().ConfigureAwait(false);
    }
}