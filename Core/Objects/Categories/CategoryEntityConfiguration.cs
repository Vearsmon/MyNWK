using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Objects.Categories;

public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");
        builder
            .HasKey(t => t.Id);

        builder
            .Property(t => t.Id)
            .ValueGeneratedOnAdd();
        
        builder
            .Property(t => t.Title)
            .HasColumnType("varchar(64)")
            .IsRequired();
    }
}