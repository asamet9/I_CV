using ICV.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICV.Infrastructure.Persistence.Configurations
{
    public class SkillDevelopmentGoalConfiguration
        : IEntityTypeConfiguration<SkillDevelopmentGoal>
    {
        public void Configure(
            EntityTypeBuilder<SkillDevelopmentGoal> builder)
        {
            builder.ToTable(nameof(SkillDevelopmentGoal));

            // ---------------------------------------------------------
            // PRIMARY KEY
            // ---------------------------------------------------------

            builder.HasKey(x => x.Id);


            // ---------------------------------------------------------
            // SKILL
            // ---------------------------------------------------------

            // Geliştirilmek istenen skill.
            // Örn: CSS, Docker, React, Azure...
            builder.Property(x => x.SkillName)
                .IsRequired()
                .HasMaxLength(100);


            // ---------------------------------------------------------
            // ENUM ALANLARI
            // ---------------------------------------------------------

            // Kullanıcının mevcut seviyesi.
            builder.Property(x => x.CurrentLevel)
                .IsRequired();

            // AI tarafından önerilen hedef seviye.
            builder.Property(x => x.RecommendedTargetLevel)
                .IsRequired();

            // Kullanıcının seçtiği hedef seviye.
            builder.Property(x => x.TargetLevel)
                .IsRequired();

            // Kullanıcının seçtiği gelişim süresi.
            builder.Property(x => x.PreferredDuration)
                .IsRequired();

            // Hedefin durumu.
            builder.Property(x => x.Status)
                .IsRequired();


            // ---------------------------------------------------------
            // COURSE PREFERENCES
            // ---------------------------------------------------------

            // Kullanıcı ücretli eğitimlere açık mı?
            builder.Property(x => x.WantsPaidCourse)
                .IsRequired();

            // Kullanıcı sertifika istiyor mu?
            builder.Property(x => x.WantsCertificate)
                .IsRequired();


            // ---------------------------------------------------------
            // PURPOSE
            // ---------------------------------------------------------

            // Kullanıcının bu skill'i neden geliştirmek istediği.
            builder.Property(x => x.Purpose)
                .HasMaxLength(500);


            // ---------------------------------------------------------
            // USER İLİŞKİSİ
            // ---------------------------------------------------------

            // Bir kullanıcı birden fazla skill geliştirme
            // hedefi oluşturabilir.
            //
            // User
            //   ├── Docker
            //   ├── CSS
            //   └── Azure
            builder.HasOne(x => x.User)
                .WithMany(x => x.SkillDevelopmentGoals)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            // ---------------------------------------------------------
            // SKILL SUGGESTION İLİŞKİSİ
            // ---------------------------------------------------------

            // Hedef bir AI önerisinden oluşturulabilir.
            //
            // SkillSuggestionId nullable olduğu için kullanıcı
            // kendi istediği skill'i de hedef olarak ekleyebilir.
            builder.HasOne(x => x.SkillSuggestion)
                .WithMany(x => x.SkillDevelopmentGoals)
                .HasForeignKey(x => x.SkillSuggestionId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}