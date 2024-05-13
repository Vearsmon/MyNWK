using Core.Objects.Categories;
using Core.Objects.Markets;
using Core.Objects.Orders;
using Core.Objects.Products;
using Core.Objects.Users;
using Core.Services.Products;

namespace Core.Objects.MyNwkUnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    public IRepository<User> UsersRepository { get; }
    public IRepository<Product> ProductRepository { get; }
    public IRepository<Market> MarketsRepository { get; }
    public IRepository<MarketInfo> MarketInfosRepository { get; }
    public IRepository<Category> CategoriesRepository { get; }
    public IRepository<Order> OrdersRepository { get; }
    public IRepository<ClickOnProductMetric> ClickOnProductMetricRepository { get; }

    public Task CommitAsync(CancellationToken cancellationToken);
}