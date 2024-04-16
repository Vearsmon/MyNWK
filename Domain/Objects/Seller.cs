namespace Domain.Objects;

public class Seller
{
    public int SellerId { get; }
    
    public int UserId { get; }
    
    public int RoomId { get; }
    
    public bool ShowRoom { get; }
    
    public Seller(int sellerId, int userId, int roomId, bool showRoom)
    {
        SellerId = sellerId;
        UserId = userId;
        RoomId = roomId;
        ShowRoom = showRoom;
    }
}