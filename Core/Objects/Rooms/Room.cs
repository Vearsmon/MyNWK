using Microsoft.EntityFrameworkCore;

namespace Core.Objects.Rooms;

[EntityTypeConfiguration(typeof(RoomEntityConfiguration))]
public class Room
{
    public int Id { get; set; }

    public int FrameNumber { get; set; }
    
    public int RoomNumber { get; set; }
    
    public char Section { get; set; }
}