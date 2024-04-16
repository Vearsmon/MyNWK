namespace Domain.Objects;

public class User
{
    public int Id { get; }
    
    public long TelegramId { get; }
    
    public string TelegramUsername { get; }
    
    public string? Name { get; }
    
    public string? PhoneNumber { get; }

    public User(
        int id,
        long telegramId,
        string telegramUsername, 
        string? name = null,
        string? phoneNumber = null)
    {
        Id = id;
        TelegramId = telegramId;
        TelegramUsername = telegramUsername;
        Name = name;
        PhoneNumber = phoneNumber;
    }
}