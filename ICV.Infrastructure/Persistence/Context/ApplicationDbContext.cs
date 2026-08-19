using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using ICV.Domain.Entities;

namespace ICV.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : DbContext //EF Core'a diyoruz ki: "Bu sınıf benim veritabanımı temsil ediyor."


    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) 
        {
        }
        //^ Bu satır sayesinde Program.cs içinde SQL Server bağlantısını vereceğiz.

        public DbSet<User> Users { get; set; }

        public DbSet<Profession> Professions { get; set; }

        public DbSet<Cv> Cvs { get; set; }

        public DbSet<CvSection> CvSections { get; set; }

        public DbSet<CvSectionItem> CvSectionItems { get; set; }

        public DbSet<SkillSuggestion> SkillSuggestions { get; set; }

        public DbSet<CourseRecommendation> CourseRecommendations { get; set; }

        public DbSet<QuestionTemplate> QuestionTemplates { get; set; }

        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<UserCvAnswer> UserCvAnswers { get; set; }

        public DbSet<UserSkillProgress> UserSkillProgresses { get; set; }

        public DbSet<SkillDevelopmentGoal> SkillDevelopmentGoals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) //Bu metodu EF Core, veritabanı modelini oluştururken otomatik çağırır.
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly); //"ApplicationDbContext'in bulunduğu projedeki bütün IEntityTypeConfiguration<> sınıflarını bul ve uygula."


        }


    }
}
