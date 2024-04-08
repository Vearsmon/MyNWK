namespace Core.Repositories.Markets;

public class MarketsRepository : IMarketsRepository
{
    private readonly MarketContext marketContext;

    public MarketsRepository(MarketContext marketContext)
    {
        this.marketContext = marketContext;
    }
}