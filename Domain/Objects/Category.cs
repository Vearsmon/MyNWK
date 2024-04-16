namespace Domain.Objects;

public class Category
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public Category(int id, string title)
    {
        Id = id;
        Title = title;
    }
}