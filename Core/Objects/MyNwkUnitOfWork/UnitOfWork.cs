using Core.Objects.Categories;
using Core.Objects.Markets;
using Core.Objects.Products;
using Core.Objects.Rooms;
using Core.Objects.Sellers;
using Core.Objects.Users;

namespace Core.Objects.MyNwkUnitOfWork;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly MyNwkDbContext myNwkDbContext;
    
    private readonly Lazy<IRepository<User>> usersRepository;
    private readonly Lazy<IRepository<Seller>> sellersRepository;
    private readonly Lazy<IRepository<Room>> roomsRepository;
    private readonly Lazy<IRepository<Product>> productRepository;
    private readonly Lazy<IRepository<Market>> marketsRepository;
    private readonly Lazy<IRepository<MarketInfo>> marketInfosRepository;
    private readonly Lazy<IRepository<Category>> categoriesInfosRepository;

    public IRepository<User> UsersRepository => usersRepository.Value;
    public IRepository<Seller> SellersRepository => sellersRepository.Value;
    public IRepository<Room> RoomsRepository => roomsRepository.Value;
    public IRepository<Product> ProductRepository => productRepository.Value;
    public IRepository<Market> MarketsRepository => marketsRepository.Value;
    public IRepository<MarketInfo> MarketInfosRepository => marketInfosRepository.Value;
    public IRepository<Category> CategoriesInfosRepository => categoriesInfosRepository.Value;

    public UnitOfWork(MyNwkDbContext myNwkDbContext)
    {
        this.myNwkDbContext = myNwkDbContext;
        usersRepository = new Lazy<IRepository<User>>(() => new Repository<User>(myNwkDbContext));
        sellersRepository = new Lazy<IRepository<Seller>>(() => new Repository<Seller>(myNwkDbContext));
        roomsRepository = new Lazy<IRepository<Room>>(() => new Repository<Room>(myNwkDbContext));
        productRepository = new Lazy<IRepository<Product>>(() => new Repository<Product>(myNwkDbContext));
        marketsRepository = new Lazy<IRepository<Market>>(() => new Repository<Market>(myNwkDbContext));
        marketInfosRepository = new Lazy<IRepository<MarketInfo>>(() => new Repository<MarketInfo>(myNwkDbContext));
        categoriesInfosRepository = new Lazy<IRepository<Category>>(() => new Repository<Category>(myNwkDbContext));
    }

    public Task CommitAsync(CancellationToken cancellationToken) => myNwkDbContext.SaveChangesAsync(cancellationToken); 

    public async ValueTask DisposeAsync()
    {
        await myNwkDbContext.SaveChangesAsync().ConfigureAwait(false);
        await myNwkDbContext.DisposeAsync().ConfigureAwait(false);
    }
}