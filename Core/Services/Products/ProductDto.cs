namespace Core.Services.Products;

public class ProductDto
{
    public ProductFullId FullId { get; set; } = null!;
    public int? CategoryId { get; set; }
    public string Title { get; set; } = null!;
    public double Price { get; set; }
    public int Remained { get; set; }
    public string? ImageRef { get; set; }
    public string Description { get; set; }
}