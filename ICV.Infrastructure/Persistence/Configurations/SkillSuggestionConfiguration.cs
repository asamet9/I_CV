using ICV.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICV.Infrastructure.Persistence.Configurations
{
    public class SkillSuggestionConfiguration : IEntityTypeConfiguration<SkillSuggestion>
    {
        public void Configure(EntityTypeBuilder<SkillSuggestion> builder)
        {
            builder.ToTable(nameof(SkillSuggestion));

            builder.HasKey(x => x.Id);

            // Önerilen yetenek
            builder.Property(x => x.SuggestedSkill)
                .IsRequired()
                .HasMaxLength(100);

            // AI açıklaması
            builder.Property(x => x.Reason)
                .HasMaxLength(1000);

            // Kategori
            builder.Property(x => x.Category)
                .HasMaxLength(50);

            // Her öneri bir CV'ye aittir.
            builder.HasOne(x => x.Cv)
         .WithMany(x => x.SkillSuggestions)
         .HasForeignKey(x => x.CvId)
         .OnDelete(DeleteBehavior.Cascade);

            // Her önerinin birçok kurs önerisi olabilir.
            builder.HasMany(x => x.CourseRecommendations)
                .WithOne(x => x.SkillSuggestion)
                .HasForeignKey(x => x.SkillSuggestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}