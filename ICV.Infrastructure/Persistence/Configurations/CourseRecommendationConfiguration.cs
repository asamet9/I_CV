using ICV.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICV.Infrastructure.Persistence.Configurations
{
    public class CourseRecommendationConfiguration
        : IEntityTypeConfiguration<CourseRecommendation>
    {
        public void Configure(EntityTypeBuilder<CourseRecommendation> builder)
        {
            builder.ToTable(nameof(CourseRecommendation));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.Provider)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Url)
                .IsRequired()
                .HasMaxLength(1000);


            // Her kurs önerisi bir SkillSuggestion'a aittir.
            builder.HasOne(x => x.SkillSuggestion)
                .WithMany(x => x.CourseRecommendations)
                .HasForeignKey(x => x.SkillSuggestionId)
                .OnDelete(DeleteBehavior.Cascade);


            // Her kurs önerisi bir Course'a bağlıdır.
            builder.HasOne(x => x.Course)
                .WithMany(x => x.CourseRecommendations)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}