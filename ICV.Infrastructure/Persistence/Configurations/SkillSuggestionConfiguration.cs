using ICV.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICV.Infrastructure.Persistence.Configurations
{
    public class SkillSuggestionConfiguration
        : IEntityTypeConfiguration<SkillSuggestion>
    {
        public void Configure(EntityTypeBuilder<SkillSuggestion> builder)
        {
            builder.ToTable(nameof(SkillSuggestion));

            // ---------------------------------------------------------
            // PRIMARY KEY
            // ---------------------------------------------------------

            builder.HasKey(x => x.Id);


            // ---------------------------------------------------------
            // SKILL
            // ---------------------------------------------------------

            builder.Property(x => x.SuggestedSkill)
                .IsRequired()
                .HasMaxLength(100);


            // ---------------------------------------------------------
            // REASON
            // ---------------------------------------------------------

            builder.Property(x => x.Reason)
                .IsRequired()
                .HasMaxLength(1000);


            // ---------------------------------------------------------
            // CATEGORY
            // ---------------------------------------------------------

            builder.Property(x => x.Category)
                .HasMaxLength(50);


            // ---------------------------------------------------------
            // RECOMMENDED TARGET LEVEL
            // ---------------------------------------------------------

            builder.Property(x => x.RecommendedTargetLevel)
                .IsRequired();


            // ---------------------------------------------------------
            // STATUS
            // ---------------------------------------------------------

            builder.Property(x => x.Status)
                .IsRequired();


            // ---------------------------------------------------------
            // CV İLİŞKİSİ
            // ---------------------------------------------------------

            builder.HasOne(x => x.Cv)
                .WithMany(x => x.SkillSuggestions)
                .HasForeignKey(x => x.CvId)
                .OnDelete(DeleteBehavior.Cascade);


            // ---------------------------------------------------------
            // USER SKILL PROGRESS İLİŞKİSİ
            // ---------------------------------------------------------

            builder.HasMany(x => x.UserSkillProgresses)
                .WithOne(x => x.SkillSuggestion)
                .HasForeignKey(x => x.SkillSuggestionId)
                .OnDelete(DeleteBehavior.Cascade);


            // ---------------------------------------------------------
            // SKILL DEVELOPMENT GOAL İLİŞKİSİ
            // ---------------------------------------------------------

            builder.HasMany(x => x.SkillDevelopmentGoals)
                .WithOne(x => x.SkillSuggestion)
                .HasForeignKey(x => x.SkillSuggestionId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}