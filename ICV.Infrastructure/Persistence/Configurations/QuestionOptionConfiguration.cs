using ICV.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICV.Infrastructure.Persistence.Configurations
{
    public class QuestionOptionConfiguration
        : IEntityTypeConfiguration<QuestionOption>
    {
        public void Configure(EntityTypeBuilder<QuestionOption> builder)
        {
            builder.ToTable(nameof(QuestionOption));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OptionText)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.OptionValue)
                .HasMaxLength(200);

            builder.HasOne(x => x.QuestionTemplate)
                .WithMany(x => x.Options)
                .HasForeignKey(x => x.QuestionTemplateId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}