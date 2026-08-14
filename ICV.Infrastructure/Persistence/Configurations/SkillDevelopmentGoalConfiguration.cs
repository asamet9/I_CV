using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            // Primary Key
            builder.HasKey(x => x.Id);

            // Kullanıcının geliştirmek istediği yetenek.
            // Örneğin: Docker, React, Azure...
            builder.Property(x => x.SkillName)
                .IsRequired()
                .HasMaxLength(100);

            // Kullanıcının haftalık ayıracağı süre.
            builder.Property(x => x.WeeklyHours)
                .IsRequired();

            // Öğrenme amacı.
            builder.Property(x => x.Purpose)
                .HasMaxLength(500);

            // ---------------------------------------------------------
            // USER İLİŞKİSİ
            // ---------------------------------------------------------

            // Bir kullanıcı birden fazla skill geliştirme hedefi
            // oluşturabilir.
            //
            // Örnek:
            // Ali -> Docker
            // Ali -> Azure
            // Ali -> React
            builder.HasOne(x => x.User)
                .WithMany(x => x.SkillDevelopmentGoals)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------------------------------------
            // SKILL SUGGESTION İLİŞKİSİ
            // ---------------------------------------------------------

            // Bir hedef bir AI önerisinden oluşturulabilir.
            // Ancak kullanıcı kendi istediği skill'i de ekleyebileceği
            // için SkillSuggestionId nullable'dır.
            builder.HasOne(x => x.SkillSuggestion)
                .WithMany(x => x.SkillDevelopmentGoals)
                .HasForeignKey(x => x.SkillSuggestionId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}