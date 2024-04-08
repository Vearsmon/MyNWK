using Core.Repositories.Rooms;
using Core.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Repositories.Sellers;

public class SellerEntityConfiguration : IEntityTypeConfiguration<SellerEntity>
{
    public void Configure(EntityTypeBuilder<SellerEntity> builder)
    {
        builder.ToTable("sellers");
        builder.HasKey(t => new 
        {
           t.SellerId,
           t.UserId
        });
        
        builder
            .Property(t => t.SellerId)
            .ValueGeneratedOnAdd();

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
            .HasOne<UserEntity>(t => t.User)
            .WithOne(t => t.Seller)
            .HasForeignKey<SellerEntity>(t => t.SellerId);

        builder
            .HasOne<RoomEntity>(t => t.Room)
            .WithOne()
            .HasForeignKey<SellerEntity>(t => t.RoomId);
    }
}