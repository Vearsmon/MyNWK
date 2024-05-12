namespace Core.Services.Products;

public class ProductToCreateDto
{
    public int? CategoryId { get; init; }
    public string Title { get; init; } = null!;
    public double Price { get; init; }
    public int Count { get; init; }
    public string? ImageLocation { get; init; }
    public string? Description { get; init; }
}