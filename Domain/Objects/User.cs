namespace Domain.Objects;

public class User
{
    public int Id { get; }
    
    public string Email { get; }
    
    public string Password { get; }
    
    public string? TelegramUsername { get; }
    
    public string? Name { get; }
    
    public string? PhoneNumber { get; }

    public User(
        int id,
        string email,
        string password,
        string? telegramUsername = null, 
        string? name = null,
        string? phoneNumber = null)
    {
        Id = id;
        Email = email;
        Password = password;
        TelegramUsername = telegramUsername;
        Name = name;
        PhoneNumber = phoneNumber;
    }
}