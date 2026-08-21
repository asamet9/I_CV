using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ICV.Domain.Common;
using ICV.Domain.Entities;


namespace ICV.Infrastructure.Persistence.Configurations
{
    public class CvConfiguration : IEntityTypeConfiguration<Cv>
    {
        public void Configure(EntityTypeBuilder<Cv> builder)
        {

            builder.ToTable(nameof(Cv));

            builder.HasKey(x => x.Id);

            builder.Property(x=>x.Title).IsRequired().HasMaxLength(100);  

            builder.Property(x=>x.Summary).HasMaxLength(1000);


            builder.HasOne(x => x.User)
                .WithMany(x => x.Cvs).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Profession)
                .WithMany(x => x.Cvs).HasForeignKey(x => x.ProfessionId);


            builder.HasMany(x => x.Sections)
                .WithOne(x => x.Cv).HasForeignKey(x => x.CvId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.SkillSuggestions).WithOne(x => x.Cv).HasForeignKey(x => x.CvId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.File)
    .WithOne(x => x.Cv)
    .HasForeignKey<CvFile>(x => x.CvId)
    .OnDelete(DeleteBehavior.Cascade);



        }
    }
}
