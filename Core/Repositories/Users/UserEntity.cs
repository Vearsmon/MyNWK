using Core.Repositories.Sellers;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories.Users;

[EntityTypeConfiguration(typeof(UserEntityConfiguration))]
public class UserEntity
{
    public int Id { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public string? TelegramUsername { get; set; }

    public string? Name { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public virtual SellerEntity? Seller { get; set; }
}