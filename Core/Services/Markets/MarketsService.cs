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

    private static MarketDto Convert(Market market) =>
        new()
        {
            Id = market.Id,
            OwnerId = market.OwnerId,
            Name = market.Name
        };
}