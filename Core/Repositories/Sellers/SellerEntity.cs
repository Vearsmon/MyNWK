using Core.Repositories.Markets;
using Core.Repositories.Rooms;
using Core.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories.Sellers;

[EntityTypeConfiguration(typeof(SellerEntityConfiguration))]
public class SellerEntity
{
    public int SellerId { get; set; }
    
    public int UserId { get; set; }
    public virtual UserEntity User { get; set; }
    
    public int RoomId { get; set; }
    public virtual RoomEntity Room { get; set; }
    
    public bool ShowRoom { get; set; }

    public virtual List<MarketEntity> Markets { get; set; }
}