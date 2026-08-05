using ICV.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICV.Infrastructure.Persistence.Configurations
{
    public class UserSkillProgressConfiguration : IEntityTypeConfiguration<UserSkillProgress>
    {
        public void Configure(EntityTypeBuilder<UserSkillProgress> builder)
        {
            builder.ToTable(nameof(UserSkillProgress));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CheckIntervalDays)
                .IsRequired();

            // Her kayıt bir kullanıcıya aittir.
            builder.HasOne(x => x.User)
                .WithMany(x => x.SkillProgresses)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Her kayıt bir SkillSuggestion'a aittir.
            builder.HasOne(x => x.SkillSuggestion)
    .WithMany(x => x.UserSkillProgresses)
    .HasForeignKey(x => x.SkillSuggestionId)
    .OnDelete(DeleteBehavior.Cascade);


        }
    }
}