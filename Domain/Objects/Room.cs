namespace Domain.Objects;

public class Room
{
    public int Id { get; }

    public int Number { get; }
    
    public int RoomNumber { get; }
    
    public char Section { get; }
    
    public Room(int id, int number, int roomNumber, char section)
    {
        Id = id;
        Number = number;
        RoomNumber = roomNumber;
        Section = section;
    }
}