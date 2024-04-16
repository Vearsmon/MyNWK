namespace Domain.Objects;

public class Market
{
    public int Id { get; set; }
    
    public Seller Owner { get; set; }
    
    public string Name { get; set; }
    
    public bool Closed { get; set; }

    public MarketInfo MarketInfo { get; set; }
    
    public List<Product> Products { get; set; }
    
    public Market(
        int id,
        Seller owner,
        string name,
        bool closed,
        MarketInfo marketInfo,
        List<Product> products)
    {
        Id = id;
        Owner = owner;
        Name = name;
        Closed = closed;
        MarketInfo = marketInfo;
        Products = products;
    }
}