using Microsoft.EntityFrameworkCore;

namespace Core.Repositories.Products;

public class ProductsRepository : IProductsRepository
{
    private readonly ProductContext productContext;

    public ProductsRepository(ProductContext productContext)
    {
        this.productContext = productContext;
    }

    public IQueryable<ProductEntity> GetProductEntities()
    {
        return productContext.Products;
    }

    public ProductEntity GetProductEntityById(int id)
    {
        return productContext.Products.FirstOrDefault(p => p.ProductId == id);
    }

    public void SaveProductEntity(ProductEntity entity)
    {
        if (entity.ProductId == default)
        {
            productContext.Entry(entity).State = EntityState.Added;
        }
        else
        {
            productContext.Entry(entity).State = EntityState.Modified;
        }

        productContext.SaveChanges();
    }

    public void DeleteProductEntityById(int id)
    {
        productContext.Products.Remove(new ProductEntity { ProductId = id });
        productContext.SaveChanges();
    }
}