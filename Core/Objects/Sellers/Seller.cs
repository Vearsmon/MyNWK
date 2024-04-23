using Core.Objects.Markets;
using Core.Objects.Rooms;
using Core.Objects.Users;
using Microsoft.EntityFrameworkCore;

namespace Core.Objects.Sellers;

[EntityTypeConfiguration(typeof(SellerEntityConfiguration))]
public class Seller
{
    public int UserId { get; set; }
    public virtual User User { get; set; }
    
    public int RoomId { get; set; }
    public virtual Room Room { get; set; }
    
    public bool ShowRoom { get; set; }

    public virtual List<Market> Markets { get; set; }
}