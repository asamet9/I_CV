using ICV.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICV.Infrastructure.Persistence.Configurations
{
    public class CvFileConfiguration : IEntityTypeConfiguration<CvFile>
    {
        public void Configure(EntityTypeBuilder<CvFile> builder)
        {
            builder.ToTable(nameof(CvFile));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OriginalFileName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.StoredFileName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.StoragePath)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.ContentType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.FileSize)
                .IsRequired();

            builder.HasOne(x => x.Cv)
                .WithOne(x => x.File)
                .HasForeignKey<CvFile>(x => x.CvId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}