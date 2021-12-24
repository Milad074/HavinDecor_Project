using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopManagement.Domain.SlideAgg;

namespace ShopManagement.Infrastructure.EFCore.Mapping
{
    public class SlideMapping : IEntityTypeConfiguration<Slide>
    {
        public void Configure(EntityTypeBuilder<Slide> builder)
        {
            builder.ToTable("Slides");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Picture).HasMaxLength(1000).IsRequired();
            builder.Property(x => x.PictureAlt).HasMaxLength(400).IsRequired();
            builder.Property(x => x.PictureTitle).HasMaxLength(600).IsRequired();
            builder.Property(x => x.Heading).HasMaxLength(250).IsRequired();
            builder.Property(x => x.Title).HasMaxLength(300);
            builder.Property(x => x.Text).HasMaxLength(700);
            builder.Property(x => x.BtnText).HasMaxLength(100).IsRequired();
            builder.Property(x => x.BtnColor).HasMaxLength(200);
            
        }
    }
}
