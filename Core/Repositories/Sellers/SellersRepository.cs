namespace Core.Repositories.Sellers;

public class SellersRepository : ISellersRepository
{
    private readonly SellerContext sellerContext;

    public SellersRepository(SellerContext sellerContext)
    {
        this.sellerContext = sellerContext;
    }
}