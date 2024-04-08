namespace Core.Repositories.Rooms;

public class RoomsRepository : IRoomsRepository
{
    private readonly RoomContext roomContext;

    public RoomsRepository(RoomContext roomContext)
    {
        this.roomContext = roomContext;
    }
}