using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopManagement.Domain.ProductAgg;

namespace ShopManagement.Infrastructure.EFCore.Mapping
{
    public class ProductMapping :IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name).HasMaxLength(300).IsRequired();
            builder.Property(p => p.Code).HasMaxLength(50);
            builder.Property(p => p.Material).HasMaxLength(100);
            builder.Property(p => p.Pieces).HasMaxLength(50);
            builder.Property(p => p.Area).HasMaxLength(200);
            builder.Property(p => p.ShortDescription).HasMaxLength(300);
            builder.Property(p => p.Description).HasMaxLength(800);
            builder.Property(p => p.Picture).HasMaxLength(1000);
            builder.Property(p => p.PictureAlt).HasMaxLength(200);
            builder.Property(p => p.PictureTitle).HasMaxLength(500);
            builder.Property(p => p.Slug).HasMaxLength(350).IsRequired();
            builder.Property(p => p.Keywords).HasMaxLength(80).IsRequired();
            builder.Property(p => p.MetaDescription).HasMaxLength(300).IsRequired();

            builder.HasOne(p => p.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.CategoryId);

            builder.HasMany(p => p.ProductPictures)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId);
        }
    }
}
