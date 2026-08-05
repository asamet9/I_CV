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
    public class ProfessionConfiguration : IEntityTypeConfiguration<Profession>
    {

        public void Configure(EntityTypeBuilder<Profession> builder)
        {
            builder.ToTable(nameof(Profession));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
       .IsRequired()
       .HasMaxLength(100);


            builder.HasMany(x => x.Cvs)
       .WithOne(x => x.Profession)
       .HasForeignKey(x => x.ProfessionId)
       .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.QuestionTemplates)
    .WithOne(x => x.Profession)
    .HasForeignKey(x => x.ProfessionId)
    .OnDelete(DeleteBehavior.Cascade);




        }



    }
}
