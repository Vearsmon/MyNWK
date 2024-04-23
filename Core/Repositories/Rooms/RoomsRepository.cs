using Microsoft.EntityFrameworkCore;

namespace Core.Repositories.Rooms;

public class RoomsRepository : IRoomsRepository
{
    private readonly RoomContext roomContext;

    public RoomsRepository(RoomContext roomContext)
    {
        this.roomContext = roomContext;
    }

    public IQueryable<RoomEntity> GetRoomEntities()
    {
        return roomContext.Rooms;
    }

    public RoomEntity GetRoomEntityById(int id)
    {
        return roomContext.Rooms.FirstOrDefault(r => r.Id == id);
    }

    public void SaveRoomEntity(RoomEntity entity)
    {
        if (entity.Id == default)
        {
            roomContext.Entry(entity).State = EntityState.Added;
        }
        else
        {
            roomContext.Entry(entity).State = EntityState.Modified;
        }

        roomContext.SaveChanges();
    }

    public void DeleteRoomEntityById(int id)
    {
        roomContext.Rooms.Remove(new RoomEntity { Id = id });
        roomContext.SaveChanges();
    }
}