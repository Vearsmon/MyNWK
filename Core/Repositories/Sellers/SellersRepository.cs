using Core.Repositories.Products;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories.Sellers;

public class SellersRepository : ISellersRepository
{
    private readonly SellerContext sellerContext;

    public SellersRepository(SellerContext sellerContext)
    {
        this.sellerContext = sellerContext;
    }

    public IQueryable<SellerEntity> GetSellerEntities()
    {
        return sellerContext.Sellers;
    }

    public SellerEntity GetSellerEntityById(int id)
    {
        return sellerContext.Sellers.FirstOrDefault(s => s.SellerId == id);
    }

    public void SaveSellerEntity(SellerEntity entity)
    {
        if (entity.SellerId == default)
        {
            sellerContext.Entry(entity).State = EntityState.Added;
        }
        else
        {
            sellerContext.Entry(entity).State = EntityState.Modified;
        }

        sellerContext.SaveChanges();
    }

    public void DeleteSellerEntityById(int id)
    {
        sellerContext.Sellers.Remove(new SellerEntity { SellerId = id });
        sellerContext.SaveChanges();
    }
}