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

        public DbSet<User> Users => Set<User>();
        public DbSet<Profession> Professions => Set<Profession>();
         public DbSet<QuestionTemplate> QuestionTemplates => Set<QuestionTemplate>(); // QuestionTemplates tablosunu temsil eder.

        public DbSet<Cv> Cvs => Set<Cv>();

        public DbSet<CvSection> CvSections => Set<CvSection>();

        public DbSet<CvSectionItem> CvSectionItems => Set<CvSectionItem>();

        public DbSet<SkillSuggestion> SkillSuggestions => Set<SkillSuggestion>();
    }
}
