namespace Core.Repositories.Markets;

public interface IMarketsRepository
{
    IQueryable<MarketEntity> GetMarketEntities();
    MarketEntity GetMarketEntityById(int id);
    void SaveMarketEntity(MarketEntity entity);
    void DeleteMarketEntityById(int id);
}