using ICV.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICV.Infrastructure.Persistence.Configurations
{
    public class QuestionTemplateConfiguration : IEntityTypeConfiguration<QuestionTemplate>
    {
        public void Configure(EntityTypeBuilder<QuestionTemplate> builder)
        {
            builder.ToTable(nameof(QuestionTemplate));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Question)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.QuestionType)
                .IsRequired()
                .HasMaxLength(50);

            // Her soru bir mesleğe aittir.
            builder.HasOne(x => x.Profession)
             .WithMany(x => x.QuestionTemplates)
             .HasForeignKey(x => x.ProfessionId)
             .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.ExpectedValue)
                .HasMaxLength(200);

            builder.Property(x => x.Category)
                .HasMaxLength(50);
        }
    }
}