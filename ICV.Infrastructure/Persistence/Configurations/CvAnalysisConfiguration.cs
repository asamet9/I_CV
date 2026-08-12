using ICV.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICV.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// CvAnalysis entity'sinin veritabanındaki
    /// tablo ve ilişkilerini yapılandırır.
    ///
    /// Entity sınıfında iş mantığını,
    /// Configuration sınıfında ise
    /// veritabanı kurallarını tutuyoruz.
    /// </summary>
    public class CvAnalysisConfiguration
        : IEntityTypeConfiguration<CvAnalysis>
    {
        public void Configure(
            EntityTypeBuilder<CvAnalysis> builder)
        {
            // -----------------------------------------------------
            // TABLO ADI
            // -----------------------------------------------------

            // Veritabanında oluşacak tablonun adını belirliyoruz.
            builder.ToTable(nameof(CvAnalysis));


            // -----------------------------------------------------
            // PRIMARY KEY
            // -----------------------------------------------------

            // CvAnalysis tablosunun Primary Key'i Id olacak.
            //
            // Id zaten BaseEntity'den geliyor.
            builder.HasKey(x => x.Id);


            // -----------------------------------------------------
            // CV İLİŞKİSİ
            // -----------------------------------------------------

            // Bir CV'nin birden fazla analizi olabilir.
            //
            // Örneğin:
            //
            // CV #5
            //   ├── Computer Engineering analizi
            //   ├── Backend Developer analizi
            //   └── Software Engineer analizi
            //
            // Dolayısıyla:
            //
            // Cv 1 -----> N CvAnalysis
            //
            builder.HasOne(x => x.Cv)
                .WithMany()
                .HasForeignKey(x => x.CvId)
                .OnDelete(DeleteBehavior.Cascade);


            // -----------------------------------------------------
            // PROFESSION İLİŞKİSİ
            // -----------------------------------------------------

            // Bir meslek için birden fazla CV analizi yapılabilir.
            //
            // Örneğin:
            //
            // Computer Engineering
            //   ├── CV #1 analizi
            //   ├── CV #5 analizi
            //   └── CV #8 analizi
            //
            builder.HasOne(x => x.Profession)
                .WithMany()
                .HasForeignKey(x => x.ProfessionId)
                .OnDelete(DeleteBehavior.Restrict);


            // -----------------------------------------------------
            // SKOR
            // -----------------------------------------------------

            // Score küsuratlı değerler alabilir.
            //
            // Örneğin:
            // 75.50
            // 82.75
            //
            // decimal kullanıldığı için SQL tarafında
            // uygun precision/scale belirliyoruz.
            builder.Property(x => x.Score)
                .HasPrecision(5, 2);


            // -----------------------------------------------------
            // EŞLEŞEN SKILL SAYISI
            // -----------------------------------------------------

            // MatchedSkillCount zorunlu bir alan.
            builder.Property(x => x.MatchedSkillCount)
                .IsRequired();


            // -----------------------------------------------------
            // EKSİK SKILL SAYISI
            // -----------------------------------------------------

            // MissingSkillCount zorunlu bir alan.
            builder.Property(x => x.MissingSkillCount)
                .IsRequired();
        }
    }
}