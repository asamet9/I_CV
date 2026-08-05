using ICV.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICV.Infrastructure.Persistence.Configurations
{
    public class CvSectionItemConfiguration : IEntityTypeConfiguration<CvSectionItem>
    {
        public void Configure(EntityTypeBuilder<CvSectionItem> builder)
        {
            // Veritabanındaki tablo adı
            builder.ToTable(nameof(CvSectionItem));

            // Primary Key (Birincil Anahtar)
            builder.HasKey(x => x.Id);

            // Başlık zorunlu
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(150);

            // Açıklama isteğe bağlı
            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            // Her kayıt bir CV bölümüne aittir.
            builder.HasOne(x => x.CvSection)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.CvSectionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}