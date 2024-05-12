namespace Core.Services.Products;

public class ProductDto
{
    public ProductFullId FullId { get; init; } = null!;
    public int? CategoryId { get; init; }
    public string Title { get; init; } = null!;
    public double Price { get; init; }
    public int Remained { get; init; }
    public string? ImageRef { get; init; }
    public string Description { get; init; }
}