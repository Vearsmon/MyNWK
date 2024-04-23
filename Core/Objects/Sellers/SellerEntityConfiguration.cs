using Core.Objects.Rooms;
using Core.Objects.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Objects.Sellers;

public class SellerEntityConfiguration : IEntityTypeConfiguration<Seller>
{
    public void Configure(EntityTypeBuilder<Seller> builder)
    {
        builder.ToTable("sellers");
        builder.HasKey(t => t.UserId);

        builder
            .Property(t => t.UserId)
            .IsRequired();

        builder
            .Property(t => t.RoomId)
            .IsRequired();

        builder
            .Property(t => t.ShowRoom)
            .HasDefaultValue(false)
            .IsRequired();

        builder
            .HasOne<User>(t => t.User)
            .WithOne(t => t.Seller)
            .HasForeignKey<Seller>(t => t.UserId);

        builder
            .HasOne<Room>(t => t.Room)
            .WithOne()
            .HasForeignKey<Seller>(t => t.RoomId);
    }
}