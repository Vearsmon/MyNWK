namespace Core.Repositories.Rooms;

public interface IRoomsRepository
{
    IQueryable<RoomEntity> GetRoomEntities();
    RoomEntity GetRoomEntityById(int id);
    void SaveRoomEntity(RoomEntity entity);
    void DeleteRoomEntityById(int id);
}