using Microsoft.EntityFrameworkCore;

namespace Core.Repositories.Rooms;

[EntityTypeConfiguration(typeof(RoomEntityConfiguration))]
public class RoomEntity
{
    public int Id { get; set; }

    public int Number { get; set; }
    
    public int Room { get; set; }
    
    public char Section { get; set; }
}