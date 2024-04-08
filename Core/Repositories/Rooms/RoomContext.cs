using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories.Rooms;

public class RoomContext : MyNwkDbContextBase<RoomContext>
{
    [UsedImplicitly]
    public DbSet<RoomEntity> Rooms { get; } = null!;

    public RoomContext(DbContextOptions<RoomContext> options) 
        : base(options)
    {
    }
}