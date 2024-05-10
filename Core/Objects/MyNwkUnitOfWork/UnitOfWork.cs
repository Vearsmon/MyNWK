﻿using Core.Objects.Categories;
using Core.Objects.Markets;
using Core.Objects.Products;
using Core.Objects.Rooms;
using Core.Objects.Sellers;
using Core.Objects.Users;

namespace Core.Objects.MyNwkUnitOfWork;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly CoreDbContext coreDbContext;
    
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

    public UnitOfWork(CoreDbContext coreDbContext)
    {
        this.coreDbContext = coreDbContext;
        usersRepository = new Lazy<IRepository<User>>(() => new Repository<User>(coreDbContext));
        sellersRepository = new Lazy<IRepository<Seller>>(() => new Repository<Seller>(coreDbContext));
        roomsRepository = new Lazy<IRepository<Room>>(() => new Repository<Room>(coreDbContext));
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