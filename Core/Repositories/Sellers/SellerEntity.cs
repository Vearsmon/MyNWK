using Core.Repositories.Markets;
using Core.Repositories.Rooms;
using Core.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories.Sellers;

[EntityTypeConfiguration(typeof(SellerEntityConfiguration))]
public sealed class SellerEntity
{
    public int SellerId { get; set; }
    
    public int UserId { get; set; }
    public UserEntity User { get; set; }
    
    public int RoomId { get; set; }
    public RoomEntity Room { get; set; }
    
    public bool ShowRoom { get; set; }

    public List<MarketEntity> Markets { get; set; }
}