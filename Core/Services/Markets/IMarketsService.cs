namespace Core.Services.Markets;

public interface IMarketsService
{
    public Task<List<MarketDto>> GetAllMarkets(RequestContext requestContext);
}