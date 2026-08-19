using ICV.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICV.Infrastructure.Persistence.Configurations
{
    public class UserCvAnswerConfiguration
        : IEntityTypeConfiguration<UserCvAnswer>
    {
        public void Configure(EntityTypeBuilder<UserCvAnswer> builder)
        {
            builder.ToTable(nameof(UserCvAnswer));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Answer)
                .IsRequired()
                .HasMaxLength(2000);

            // Cevap hangi CV'ye ait?
            builder.HasOne(x => x.Cv)
                .WithMany()
                .HasForeignKey(x => x.CvId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cevap hangi soruya ait?
            builder.HasOne(x => x.QuestionTemplate)
                .WithMany()
                .HasForeignKey(x => x.QuestionTemplateId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}