using Core.Objects.Categories;
using Core.Objects.Markets;
using Core.Objects.Products;
using Core.Objects.Rooms;
using Core.Objects.Sellers;
using Core.Objects.Users;

namespace Core.Objects.MyNwkUnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    public IRepository<User> UsersRepository { get; }
    public IRepository<Seller> SellersRepository { get; }
    public IRepository<Room> RoomsRepository { get; }
    public IRepository<Product> ProductRepository { get; }
    public IRepository<Market> MarketsRepository { get; }
    public IRepository<MarketInfo> MarketInfosRepository { get; }
    public IRepository<Category> CategoriesInfosRepository { get; }

    public Task CommitAsync(CancellationToken cancellationToken);
}