using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Repositories.User;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("users");
        builder.HasKey(t => t.Id);
        
        builder
            .Property(t => t.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAddOrUpdate();

        builder
            .Property(t => t.Email)
            .HasColumnName("email")
            .HasColumnType("varchar(64)")
            .IsRequired();

        builder
            .Property(t => t.Password)
            .HasColumnName("password")
            .HasColumnType("varchar(64)")
            .IsRequired();

        builder
            .Property(t => t.TelegramUsername)
            .HasColumnName("telegram_username")
            .HasColumnType("varchar(64)")
            .IsRequired();

        builder
            .Property(t => t.Name)
            .HasColumnName("name")
            .HasColumnType("varchar(64)");

        builder
            .Property(t => t.PhoneNumber)
            .HasColumnName("phone_number")
            .HasColumnType("varchar(16)");
    }
}