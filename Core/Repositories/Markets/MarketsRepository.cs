using Microsoft.EntityFrameworkCore;

namespace Core.Repositories.Markets;

public class MarketsRepository : IMarketsRepository
{
    private readonly MarketContext marketContext;

    public MarketsRepository(MarketContext marketContext)
    {
        this.marketContext = marketContext;
    }

    public IQueryable<MarketEntity> GetMarketEntities()
    {
        return marketContext.Markets;
    }

    public MarketEntity GetMarketEntityById(int id)
    {
        return marketContext.Markets.FirstOrDefault(m => m.Id == id);
    }

    public void SaveMarketEntity(MarketEntity entity)
    {
        if (entity.Id == default)
        {
            marketContext.Entry(entity).State = EntityState.Added;
        }
        else
        {
            marketContext.Entry(entity).State = EntityState.Modified;
        }

        marketContext.SaveChanges();
    }

    public void DeleteMarketEntityById(int id)
    {
        marketContext.Markets.Remove(new MarketEntity { Id = id });
        marketContext.SaveChanges();
    }
}