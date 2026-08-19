using ICV.Domain.Entities;
using ICV.Infrastructure.Persistence.Context;

namespace ICV.Infrastructure.Persistence.Seeds
{
    public static class QuestionSeed
    {
        public static void Seed(
            ApplicationDbContext context)
        {
            // =====================================================
            // COMPUTER ENGINEERING PROFESSION
            // =====================================================

            const int professionId = 1;

            // =====================================================
            // QUESTION TEMPLATES
            // =====================================================

            var questions = new List<QuestionTemplate>
            {
                new QuestionTemplate
                {
                    Id = 100,
                    ProfessionId = professionId,
                    Question = "CV başlığınız nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Profile"
                },

                new QuestionTemplate
                {
                    Id = 101,
                    ProfessionId = professionId,
                    Question = "Kendinizi kısaca tanıtın.",
                    QuestionType = "TextArea",
                    IsRequired = false,
                    Category = "Profile"
                },

                new QuestionTemplate
                {
                    Id = 102,
                    ProfessionId = professionId,
                    Question = "En yüksek eğitim seviyeniz nedir?",
                    QuestionType = "Select",
                    IsRequired = true,
                    Category = "Education"
                },

                new QuestionTemplate
                {
                    Id = 103,
                    ProfessionId = professionId,
                    Question = "Üniversitenizin adı nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Education"
                },

                new QuestionTemplate
                {
                    Id = 104,
                    ProfessionId = professionId,
                    Question = "Bölümünüz nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Education"
                },

                new QuestionTemplate
                {
                    Id = 105,
                    ProfessionId = professionId,
                    Question = "Eğitime başlama tarihiniz nedir?",
                    QuestionType = "Date",
                    IsRequired = true,
                    Category = "Education"
                },

                new QuestionTemplate
                {
                    Id = 106,
                    ProfessionId = professionId,
                    Question = "Mezuniyet tarihiniz nedir?",
                    QuestionType = "Date",
                    IsRequired = false,
                    Category = "Education"
                },

                new QuestionTemplate
                {
                    Id = 107,
                    ProfessionId = professionId,
                    Question = "Daha önce profesyonel iş deneyiminiz oldu mu?",
                    QuestionType = "Select",
                    IsRequired = true,
                    Category = "Experience"
                },

                new QuestionTemplate
                {
                    Id = 108,
                    ProfessionId = professionId,
                    Question = "Bildiğiniz programlama dillerini seçin.",
                    QuestionType = "MultiSelect",
                    IsRequired = false,
                    Category = "Programming"
                },

                new QuestionTemplate
                {
                    Id = 109,
                    ProfessionId = professionId,
                    Question = "Bildiğiniz framework ve kütüphaneleri seçin.",
                    QuestionType = "MultiSelect",
                    IsRequired = false,
                    Category = "Programming"
                },

                new QuestionTemplate
                {
                    Id = 110,
                    ProfessionId = professionId,
                    Question = "Bildiğiniz veritabanı teknolojilerini seçin.",
                    QuestionType = "MultiSelect",
                    IsRequired = false,
                    Category = "Database"
                },

                new QuestionTemplate
                {
                    Id = 111,
                    ProfessionId = professionId,
                    Question = "Kullandığınız geliştirme araçlarını seçin.",
                    QuestionType = "MultiSelect",
                    IsRequired = false,
                    Category = "Tools"
                },

                new QuestionTemplate
                {
                    Id = 112,
                    ProfessionId = professionId,
                    Question = "Kullandığınız Cloud / DevOps teknolojilerini seçin.",
                    QuestionType = "MultiSelect",
                    IsRequired = false,
                    Category = "DevOps"
                },

                new QuestionTemplate
                {
                    Id = 113,
                    ProfessionId = professionId,
                    Question = "Daha önce geliştirdiğiniz bir proje var mı?",
                    QuestionType = "Select",
                    IsRequired = true,
                    Category = "Project"
                },

                new QuestionTemplate
                {
                    Id = 114,
                    ProfessionId = professionId,
                    Question = "Sahip olduğunuz sertifikalar var mı?",
                    QuestionType = "Select",
                    IsRequired = false,
                    Category = "Certificate"
                },

                new QuestionTemplate
                {
                    Id = 115,
                    ProfessionId = professionId,
                    Question = "Bildiğiniz yabancı dilleri seçin.",
                    QuestionType = "MultiSelect",
                    IsRequired = false,
                    Category = "Language"
                }
            };

            // =====================================================
            // EXPERIENCE QUESTIONS
            // =====================================================

            questions.AddRange(new[]
            {
                new QuestionTemplate
                {
                    Id = 116,
                    ProfessionId = professionId,
                    Question = "Şirket veya kurum adı nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Experience"
                },

                new QuestionTemplate
                {
                    Id = 117,
                    ProfessionId = professionId,
                    Question = "Pozisyonunuz nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Experience"
                },

                new QuestionTemplate
                {
                    Id = 118,
                    ProfessionId = professionId,
                    Question = "Bu pozisyondaki sorumluluklarınız nelerdi?",
                    QuestionType = "TextArea",
                    IsRequired = true,
                    Category = "Experience"
                },

                new QuestionTemplate
                {
                    Id = 119,
                    ProfessionId = professionId,
                    Question = "İşe başlama tarihiniz nedir?",
                    QuestionType = "Date",
                    IsRequired = true,
                    Category = "Experience"
                },

                new QuestionTemplate
                {
                    Id = 120,
                    ProfessionId = professionId,
                    Question = "İşten ayrılma tarihiniz nedir?",
                    QuestionType = "Date",
                    IsRequired = false,
                    Category = "Experience"
                }
            });

            // =====================================================
            // PROJECT QUESTIONS
            // =====================================================

            questions.AddRange(new[]
            {
                new QuestionTemplate
                {
                    Id = 121,
                    ProfessionId = professionId,
                    Question = "Proje adı nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Project"
                },

                new QuestionTemplate
                {
                    Id = 122,
                    ProfessionId = professionId,
                    Question = "Projeyi kısaca açıklayın.",
                    QuestionType = "TextArea",
                    IsRequired = true,
                    Category = "Project"
                },

                new QuestionTemplate
                {
                    Id = 123,
                    ProfessionId = professionId,
                    Question = "Projede kullandığınız teknolojileri yazın.",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Project"
                },

                new QuestionTemplate
                {
                    Id = 124,
                    ProfessionId = professionId,
                    Question = "Projenin başlangıç tarihi nedir?",
                    QuestionType = "Date",
                    IsRequired = false,
                    Category = "Project"
                },

                new QuestionTemplate
                {
                    Id = 125,
                    ProfessionId = professionId,
                    Question = "Projenin bitiş tarihi nedir?",
                    QuestionType = "Date",
                    IsRequired = false,
                    Category = "Project"
                }
            });

            // =====================================================
            // CERTIFICATE QUESTIONS
            // =====================================================

            questions.AddRange(new[]
            {
                new QuestionTemplate
                {
                    Id = 126,
                    ProfessionId = professionId,
                    Question = "Sertifika adı nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Certificate"
                },

                new QuestionTemplate
                {
                    Id = 127,
                    ProfessionId = professionId,
                    Question = "Sertifikayı veren kurum nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Certificate"
                },

                new QuestionTemplate
                {
                    Id = 128,
                    ProfessionId = professionId,
                    Question = "Sertifika tarihi nedir?",
                    QuestionType = "Date",
                    IsRequired = false,
                    Category = "Certificate"
                }
            });

            context.QuestionTemplates.AddRange(questions);

            // =====================================================
            // OPTIONS
            // =====================================================

            var options = new List<QuestionOption>();

            // Education
            AddOptions(options, 102,
                "Ön Lisans",
                "Lisans",
                "Yüksek Lisans",
                "Doktora");

            // Yes / No
            AddOptions(options, 107,
                "Evet",
                "Hayır");

            AddOptions(options, 113,
                "Evet",
                "Hayır");

            AddOptions(options, 114,
                "Evet",
                "Hayır");

            // Programming
            AddOptions(options, 108,
                "C#",
                "Java",
                "Python",
                "C++",
                "JavaScript",
                "TypeScript",
                "Go",
                "PHP",
                "Rust",
                "Kotlin",
                "Swift");

            // Frameworks
            AddOptions(options, 109,
                "ASP.NET Core",
                ".NET",
                "Entity Framework Core",
                "React",
                "Angular",
                "Vue.js",
                "Node.js",
                "Express.js",
                "Spring",
                "Django",
                "Flask",
                "TensorFlow",
                "PyTorch",
                "Unity");

            // Databases
            AddOptions(options, 110,
                "SQL Server",
                "PostgreSQL",
                "MySQL",
                "Oracle",
                "SQLite",
                "MongoDB",
                "Redis");

            // Tools
            AddOptions(options, 111,
                "Git",
                "GitHub",
                "GitLab",
                "Bitbucket",
                "Visual Studio",
                "Visual Studio Code",
                "JetBrains Rider",
                "Postman",
                "Swagger",
                "Jira");

            // DevOps
            AddOptions(options, 112,
                "Docker",
                "Kubernetes",
                "Azure",
                "AWS",
                "Google Cloud",
                "GitHub Actions",
                "GitLab CI/CD",
                "Jenkins",
                "Terraform",
                "Ansible");

            // Languages
            AddOptions(options, 115,
                "İngilizce",
                "Almanca",
                "Fransızca",
                "İspanyolca",
                "İtalyanca",
                "Rusça");

            context.QuestionOptions.AddRange(options);

            context.SaveChanges();
        }

        private static void AddOptions(
            List<QuestionOption> options,
            int questionTemplateId,
            params string[] values)
        {
            var order = 1;

            foreach (var value in values)
            {
                options.Add(new QuestionOption
                {
                    QuestionTemplateId = questionTemplateId,
                    OptionText = value,
                    OptionValue = value,
                    OrderIndex = order++
                });
            }
        }
    }
}