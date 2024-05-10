using Core.Helpers;
using Core.Objects.MyNwkUnitOfWork;
using Core.Objects.Products;
using Core.Objects.Sellers;
using Core.Services.Products;
using Microsoft.EntityFrameworkCore;

namespace Core.Objects.Markets;

[EntityTypeConfiguration(typeof(MarketEntityConfiguration))]
public class Market
{
    public int Id { get; set; }
    
    public int OwnerId { get; set; }
    public virtual Seller Seller { get; set; }
    
    public string Name { get; set; }
    
    public bool Closed { get; set; }

    public virtual MarketInfo MarketInfo { get; set; }
    
    public virtual List<Product> Products { get; set; }

    public async Task<ProductFullId> AddProduct(
        IUnitOfWork unitOfWork,
        ProductToCreateDto productToCreate,
        CancellationToken cancellationToken)
    {
        var product = ConvertToProduct(productToCreate);
        Products.Add(product);
        await unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
        
        return new ProductFullId(OwnerId, Id, product.ProductId);
    }
    
    private static Product ConvertToProduct(ProductToCreateDto productToCreate) => new()
        {
            CategoryId = productToCreate.CategoryId,
            CreatedAt = PreciseTimestampGenerator.Generate(),
            ImageLocation = productToCreate.ImageLocation,
            Price = productToCreate.Price,
            Remained = productToCreate.Count,
            Title = productToCreate.Title
        };
}