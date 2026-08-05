using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using ICV.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ICV.Domain.Common;



namespace ICV.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User> //"Ben User entity'sinin veritabanı ayarlarını yapacağım."
    {

        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.ToTable("Users")
            .HasKey(x => x.Id); //Bu tablonun Primary Key'i (Birincil Anahtarı) hangisi? && Id yi baseentitiy calsından kalıttık

            // builder.Property(x => x.Email).IsRequired(); //"Bu kolonun özelliklerini ayarlayacağım." && boş bırakılamaz
            //builder.Property(x => x.Email).HasMaxLength(150); max 150 harf
            //daha iyi kulalnım;

            builder.Property(x => x.Email)
               .IsRequired() 
               .HasMaxLength(150);


            // Ad Soyad en fazla 100 karakter olabilir.
            builder.Property(x => x.FullName).IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(x => x.Email).IsUnique();

            builder.Property(x => x.PreferredLanguage)
      .IsRequired()
      .HasMaxLength(5)
      .HasDefaultValue("en"); // varsayılan değer en olsun

            builder.Property(x => x.PasswordHash).HasMaxLength(500);



            builder.HasMany(x => x.Cvs)
                .WithOne(x => x.User) //HasMany (Birçoğuna Sahip)
                .HasForeignKey(x => x.UserId)  //HasForeignKey (Yabancı Anahtarı Belirle)
                .OnDelete(DeleteBehavior.Cascade); //Cascade (Zincirleme Silme) demek. && bağlı olduğu şey silinirse o da silinir




        }

    }
}
