namespace Core.Repositories.Products;

public interface IProductsRepository
{
    IQueryable<ProductEntity> GetProductEntities();
    ProductEntity GetProductEntityById(int id);
    void SaveProductEntity(ProductEntity entity);
    void DeleteProductEntityById(int id);

}