using Core.Objects.Markets;
using Microsoft.EntityFrameworkCore;

namespace Core.Objects.Users;

[EntityTypeConfiguration(typeof(UserEntityConfiguration))]
public class User
{
    public int Id { get; set; }
    
    public string Address { get; set; }
    
    public long TelegramId { get; set; }
    
    public string TelegramUsername { get; set; }

    public string? Name { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public virtual Market Market { get; set; }
}