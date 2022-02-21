using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopManagement.Domain.MaterialAgg;

namespace ShopManagement.Infrastructure.EFCore.Mapping
{
    public class MaterialMapping : IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.ToTable("Materials");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.RingColor).HasMaxLength(100).IsRequired();
            builder.Property(x => x.MaterialName).HasMaxLength(500).IsRequired();
            builder.Property(x => x.Panel).HasMaxLength(700).IsRequired();
        }
    }
}
