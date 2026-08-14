using ICV.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICV.Infrastructure.Persistence.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable(nameof(Course));

            builder.HasKey(x => x.Id);

            // Kurs adı
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            // Kurs açıklaması
            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            // Kurs sağlayıcısı
            // Örn: Udemy, Coursera, YouTube
            builder.Property(x => x.Provider)
                .IsRequired()
                .HasMaxLength(100);

            // Kurs bağlantısı
            builder.Property(x => x.Url)
                .IsRequired()
                .HasMaxLength(1000);

            // Kurs seviyesi
            builder.Property(x => x.Level)
                .IsRequired();

            // Kurs kategorisi
            builder.Property(x => x.Category)
                .HasMaxLength(100);

            // Tahmini toplam süre
            builder.Property(x => x.DurationHours);

            // Ücretsiz / ücretli
            builder.Property(x => x.IsFree)
                .IsRequired();

            // Aktif / pasif
            builder.Property(x => x.IsActive)
                .IsRequired();

            // Bir kurs birçok CourseRecommendation içerisinde
            // kullanılabilir.
            builder.HasMany(x => x.CourseRecommendations)
                .WithOne(x => x.Course)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}