namespace Core.Repositories.Products;

public class ProductsRepository : IProductsRepository
{
    private readonly ProductContext productContext;

    public ProductsRepository(ProductContext productContext)
    {
        this.productContext = productContext;
    }
}