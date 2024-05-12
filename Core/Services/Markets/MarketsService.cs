using Core.Objects.Markets;
using Core.Objects.MyNwkUnitOfWork;

namespace Core.Services.Markets;

public class MarketsService : IMarketsService
{
    private readonly IUnitOfWorkProvider unitOfWorkProvider;

    public MarketsService(IUnitOfWorkProvider unitOfWorkProvider)
    {
        this.unitOfWorkProvider = unitOfWorkProvider;
    }

    public async Task<List<MarketDto>> GetAllMarkets(RequestContext requestContext)
    {
        await using var unitOfWork = unitOfWorkProvider.Get();
        var markets = await unitOfWork.MarketsRepository.GetAsync(
                r => r
                    .OrderBy(m => m.Name)
                    .ThenBy(m => m.Id),
                requestContext.CancellationToken)
            .ConfigureAwait(false);
        return markets.Select(Convert).ToList();
    }

    public async Task<MarketInfoDto?> GetMarketInfo(RequestContext requestContext, int marketId)
    {
        await using var unitOfWork = unitOfWorkProvider.Get();

        var market = await unitOfWork.MarketsRepository.GetAsync(
                    r => r.Where(m => 
                        marketId == 0 
                            ? m.OwnerId == requestContext.UserId 
                            : m.Id == marketId),
                    requestContext.CancellationToken)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

        if (market is null)
        {
            return null;
        }

        var marketInfo = await unitOfWork.MarketInfosRepository.GetAsync(
                r => r.Where(m => m.MarketId == market.Id),
                requestContext.CancellationToken)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);

        return Convert(market, marketInfo!);
    }

    public async Task UpdateAsync(RequestContext requestContext, MarketToUpdateDto marketToUpdateDto)
    {
        await using var unitOfWork = unitOfWorkProvider.Get();
        var market = await unitOfWork.MarketsRepository.GetAsync(
                r => r.Where(m => m.Id == marketToUpdateDto.Id),
                requestContext.CancellationToken)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        
        if (market is null)
        {
            throw new InvalidOperationException($"Could not find market with id: {marketToUpdateDto.Id}");
        }

        if (market.OwnerId != requestContext.UserId)
        {
            throw new InvalidOperationException($"User is not allowed to update market with id: {marketToUpdateDto.Id}");
        }
        
        market.Closed = marketToUpdateDto.Closed;
        market.Name = marketToUpdateDto.Name;
        var marketInfo = new MarketInfo
        {
            MarketId = marketToUpdateDto.Id,
            AutoHide = marketToUpdateDto.AutoHide,
            Description = marketToUpdateDto.Description,
            WorksFrom = marketToUpdateDto.WorksFrom,
            WorksTo = marketToUpdateDto.WorksTo
        };
        unitOfWork.MarketInfosRepository.Update(marketInfo);
        await unitOfWork.CommitAsync(requestContext.CancellationToken).ConfigureAwait(false);
    }

    private static MarketDto Convert(Market market) =>
        new()
        {
            Id = market.Id,
            OwnerId = market.OwnerId,
            Name = market.Name
        };
    
    private static MarketInfoDto Convert(Market market, MarketInfo marketInfo) =>
        new()
        {
            Id = market.Id,
            OwnerId = market.OwnerId,
            Name = market.Name,
            AutoHide = marketInfo.AutoHide,
            Closed = market.Closed,
            Description = marketInfo.Description ?? "",
            WorksFrom = marketInfo.WorksFrom,
            WorksTo = marketInfo.WorksTo
        };
}