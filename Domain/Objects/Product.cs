namespace Domain.Objects;

public struct Product
{
    public int ProductId { get; }
    
    public Category? Category { get; }
    
    public string Title { get; }
    
    public string? ImageLocation { get; }
    
    public double Price { get; }
    
    public int Remained { get; }
    
    public Product(
        int productId,
        string title, 
        double price,
        int remained, 
        Category? category,
        string? imageLocation = null)
    {
        ProductId = productId;
        Category = category;
        Title = title;
        ImageLocation = imageLocation;
        Price = price;
        Remained = remained;
    }
}