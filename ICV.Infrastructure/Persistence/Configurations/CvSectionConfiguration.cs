using ICV.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICV.Infrastructure.Persistence.Configurations
{
    public class CvSectionConfiguration : IEntityTypeConfiguration<CvSection>
    {
        public void Configure(EntityTypeBuilder<CvSection> builder)
        {
            // Veritabanındaki tablo adı
            builder.ToTable(nameof(CvSection));

            // Primary Key (Birincil Anahtar)
            builder.HasKey(x => x.Id);

            // Bir CV Bölümü (CvSection) sadece bir CV'ye aittir.
            // Bir CV'nin ise birden fazla bölümü olabilir.
            builder.HasOne(x => x.Cv)
                .WithMany(x => x.Sections)
                .HasForeignKey(x => x.CvId)
                .OnDelete(DeleteBehavior.Cascade);

            // Bir CV Bölümünün (CvSection) birden fazla kaydı (CvSectionItem) olabilir.
            // Her kayıt sadece bir CV Bölümüne aittir.
            builder.HasMany(x => x.Items)
                .WithOne(x => x.CvSection)
                .HasForeignKey(x => x.CvSectionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}