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

            builder.HasOne(x => x.SkillDevelopmentGoal)
                .WithMany(x => x.CourseRecommendations)
                .HasForeignKey(x => x.SkillDevelopmentGoalId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Course)
                .WithMany(x => x.CourseRecommendations)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new
            {
                x.SkillDevelopmentGoalId,
                x.CourseId
            })
            .IsUnique();
        }
    }
}