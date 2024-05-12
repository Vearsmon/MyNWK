using Web.Models;

namespace Core.Services.Markets;

public interface IMarketsService
{
    public Task<List<MarketDto>> GetAllMarkets(RequestContext requestContext);
    
    public Task<MarketInfoDto?> GetMarketInfo(RequestContext requestContext, int marketId);

    public Task UpdateAsync(RequestContext requestContext, MarketToUpdate marketToUpdate);
}