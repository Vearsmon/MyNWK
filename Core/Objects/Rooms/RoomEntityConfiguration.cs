using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Objects.Rooms;

public class RoomEntityConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("rooms");
        builder.HasKey(t => t.Id);
        
        builder
            .Property(t => t.Id)
            .ValueGeneratedOnAdd();

        builder.Property(t => t.FrameNumber).IsRequired();
        builder.Property(t => t.RoomNumber).IsRequired();
        builder.Property(t => t.Section);
    }
}