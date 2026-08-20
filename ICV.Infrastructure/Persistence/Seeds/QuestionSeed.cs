using ICV.Domain.Entities;
using ICV.Infrastructure.Persistence.Context;

namespace ICV.Infrastructure.Persistence.Seeds
{
    public static class QuestionSeed
    {
        public static void Seed(ApplicationDbContext context)
        {
            var questions = new List<QuestionTemplate>();
            var options = new List<QuestionOption>();

            // =====================================================
            // COMPUTER ENGINEERING & SOFTWARE DEVELOPMENT
            // ProfessionId = 1
            // Question IDs = 100 - 199
            // =====================================================

            AddComputerQuestions(questions, options);

            // =====================================================
            // MECHANICAL ENGINEERING
            // ProfessionId = 2
            // Question IDs = 200 - 299
            // =====================================================

            AddMechanicalQuestions(questions, options);

            // =====================================================
            // INDUSTRIAL ENGINEERING
            // ProfessionId = 3
            // Question IDs = 300 - 399
            // =====================================================

            AddIndustrialQuestions(questions, options);

            // =====================================================
            // ELECTRICAL AND ELECTRONICS ENGINEERING
            // ProfessionId = 4
            // Question IDs = 400 - 499
            // =====================================================

            AddElectricalQuestions(questions, options);

            context.QuestionTemplates.AddRange(questions);
            context.QuestionOptions.AddRange(options);

            context.SaveChanges();
        }

        // =========================================================
        // COMPUTER ENGINEERING
        // =========================================================

        private static void AddComputerQuestions(
            List<QuestionTemplate> questions,
            List<QuestionOption> options)
        {
            const int professionId = 1;

            questions.AddRange(new[]
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
                },

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
                },

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
                },

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

            AddOptions(options, 102,
                "Ön Lisans",
                "Lisans",
                "Yüksek Lisans",
                "Doktora");

            AddOptions(options, 107,
                "Evet",
                "Hayır");

            AddOptions(options, 113,
                "Evet",
                "Hayır");

            AddOptions(options, 114,
                "Evet",
                "Hayır");

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

            AddOptions(options, 110,
                "SQL Server",
                "PostgreSQL",
                "MySQL",
                "Oracle",
                "SQLite",
                "MongoDB",
                "Redis");

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

            AddOptions(options, 115,
                "İngilizce",
                "Almanca",
                "Fransızca",
                "İspanyolca",
                "İtalyanca",
                "Rusça");
        }

        // =========================================================
        // MECHANICAL ENGINEERING
        // =========================================================

        private static void AddMechanicalQuestions(
            List<QuestionTemplate> questions,
            List<QuestionOption> options)
        {
            const int professionId = 2;

            questions.AddRange(new[]
            {
                new QuestionTemplate
                {
                    Id = 200,
                    ProfessionId = professionId,
                    Question = "CV başlığınız nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Profile"
                },

                new QuestionTemplate
                {
                    Id = 201,
                    ProfessionId = professionId,
                    Question = "Kendinizi kısaca tanıtın.",
                    QuestionType = "TextArea",
                    IsRequired = false,
                    Category = "Profile"
                },

                new QuestionTemplate
                {
                    Id = 202,
                    ProfessionId = professionId,
                    Question = "En yüksek eğitim seviyeniz nedir?",
                    QuestionType = "Select",
                    IsRequired = true,
                    Category = "Education"
                },

                new QuestionTemplate
                {
                    Id = 203,
                    ProfessionId = professionId,
                    Question = "Üniversitenizin adı nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Education"
                },

                new QuestionTemplate
                {
                    Id = 204,
                    ProfessionId = professionId,
                    Question = "Bölümünüz nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Education"
                },

                new QuestionTemplate
                {
                    Id = 205,
                    ProfessionId = professionId,
                    Question = "Daha önce profesyonel iş deneyiminiz oldu mu?",
                    QuestionType = "Select",
                    IsRequired = true,
                    Category = "Experience"
                },

                new QuestionTemplate
                {
                    Id = 206,
                    ProfessionId = professionId,
                    Question = "Bildiğiniz CAD programlarını seçin.",
                    QuestionType = "MultiSelect",
                    IsRequired = false,
                    Category = "CAD"
                },

                new QuestionTemplate
                {
                    Id = 207,
                    ProfessionId = professionId,
                    Question = "Kullandığınız üretim ve imalat teknolojilerini seçin.",
                    QuestionType = "MultiSelect",
                    IsRequired = false,
                    Category = "Manufacturing"
                },

                new QuestionTemplate
                {
                    Id = 208,
                    ProfessionId = professionId,
                    Question = "Kullandığınız mühendislik analiz programlarını seçin.",
                    QuestionType = "MultiSelect",
                    IsRequired = false,
                    Category = "Analysis"
                },

                new QuestionTemplate
                {
                    Id = 209,
                    ProfessionId = professionId,
                    Question = "Bildiğiniz simülasyon programlarını seçin.",
                    QuestionType = "MultiSelect",
                    IsRequired = false,
                    Category = "Simulation"
                },

                new QuestionTemplate
                {
                    Id = 210,
                    ProfessionId = professionId,
                    Question = "Daha önce geliştirdiğiniz bir proje var mı?",
                    QuestionType = "Select",
                    IsRequired = true,
                    Category = "Project"
                },

                new QuestionTemplate
                {
                    Id = 211,
                    ProfessionId = professionId,
                    Question = "Bildiğiniz yabancı dilleri seçin.",
                    QuestionType = "MultiSelect",
                    IsRequired = false,
                    Category = "Language"
                },

                new QuestionTemplate
                {
                    Id = 212,
                    ProfessionId = professionId,
                    Question = "Şirket veya kurum adı nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Experience"
                },

                new QuestionTemplate
                {
                    Id = 213,
                    ProfessionId = professionId,
                    Question = "Pozisyonunuz nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Experience"
                },

                new QuestionTemplate
                {
                    Id = 214,
                    ProfessionId = professionId,
                    Question = "Sorumluluklarınız nelerdi?",
                    QuestionType = "TextArea",
                    IsRequired = true,
                    Category = "Experience"
                },

                new QuestionTemplate
                {
                    Id = 215,
                    ProfessionId = professionId,
                    Question = "Proje adı nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Project"
                },

                new QuestionTemplate
                {
                    Id = 216,
                    ProfessionId = professionId,
                    Question = "Projeyi kısaca açıklayın.",
                    QuestionType = "TextArea",
                    IsRequired = true,
                    Category = "Project"
                },

                new QuestionTemplate
                {
                    Id = 217,
                    ProfessionId = professionId,
                    Question = "Projede kullandığınız teknolojileri yazın.",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Project"
                },

                new QuestionTemplate
                {
                    Id = 218,
                    ProfessionId = professionId,
                    Question = "Sahip olduğunuz sertifikalar var mı?",
                    QuestionType = "Select",
                    IsRequired = false,
                    Category = "Certificate"
                },

                new QuestionTemplate
                {
                    Id = 219,
                    ProfessionId = professionId,
                    Question = "Sertifika adı nedir?",
                    QuestionType = "Text",
                    IsRequired = false,
                    Category = "Certificate"
                }
            });

            AddOptions(options, 202,
                "Ön Lisans",
                "Lisans",
                "Yüksek Lisans",
                "Doktora");

            AddOptions(options, 205,
                "Evet",
                "Hayır");

            AddOptions(options, 206,
                "AutoCAD",
                "SolidWorks",
                "CATIA",
                "Siemens NX",
                "Creo",
                "Fusion 360");

            AddOptions(options, 207,
                "CNC",
                "CNC Milling",
                "CNC Turning",
                "Welding",
                "3D Printing",
                "Injection Molding");

            AddOptions(options, 208,
                "ANSYS",
                "Abaqus",
                "SolidWorks Simulation",
                "MATLAB",
                "COMSOL");

            AddOptions(options, 209,
                "ANSYS Fluent",
                "ANSYS Mechanical",
                "Abaqus",
                "COMSOL",
                "MATLAB Simulink");

            AddOptions(options, 210,
                "Evet",
                "Hayır");

            AddOptions(options, 211,
                "İngilizce",
                "Almanca",
                "Fransızca",
                "İspanyolca");

            AddOptions(options, 218,
                "Evet",
                "Hayır");
        }

        // =========================================================
        // INDUSTRIAL ENGINEERING
        // =========================================================

        private static void AddIndustrialQuestions(
            List<QuestionTemplate> questions,
            List<QuestionOption> options)
        {
            const int professionId = 3;

            questions.AddRange(new[]
            {
                new QuestionTemplate
                {
                    Id = 300,
                    ProfessionId = professionId,
                    Question = "CV başlığınız nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Profile"
                },

                new QuestionTemplate
                {
                    Id = 301,
                    ProfessionId = professionId,
                    Question = "Kendinizi kısaca tanıtın.",
                    QuestionType = "TextArea",
                    IsRequired = false,
                    Category = "Profile"
                },

                new QuestionTemplate
                {
                    Id = 302,
                    ProfessionId = professionId,
                    Question = "En yüksek eğitim seviyeniz nedir?",
                    QuestionType = "Select",
                    IsRequired = true,
                    Category = "Education"
                },

                new QuestionTemplate
                {
                    Id = 303,
                    ProfessionId = professionId,
                    Question = "Üniversitenizin adı nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Education"
                },

                new QuestionTemplate
                {
                    Id = 304,
                    ProfessionId = professionId,
                    Question = "Bölümünüz nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Education"
                },

                new QuestionTemplate
                {
                    Id = 305,
                    ProfessionId = professionId,
                    Question = "Daha önce profesyonel iş deneyiminiz oldu mu?",
                    QuestionType = "Select",
                    IsRequired = true,
                    Category = "Experience"
                },

                new QuestionTemplate
                {
                    Id = 306,
                    ProfessionId = professionId,
                    Question = "Bildiğiniz ERP sistemlerini seçin.",
                    QuestionType = "MultiSelect",
                    IsRequired = false,
                    Category = "ERP"
                },

                new QuestionTemplate
                {
                    Id = 307,
                    ProfessionId = professionId,
                    Question = "Bildiğiniz veri analizi araçlarını seçin.",
                    QuestionType = "MultiSelect",
                    IsRequired = false,
                    Category = "DataAnalysis"
                },

                new QuestionTemplate
                {
                    Id = 308,
                    ProfessionId = professionId,
                    Question = "Bildiğiniz optimizasyon yöntem ve araçlarını seçin.",
                    QuestionType = "MultiSelect",
                    IsRequired = false,
                    Category = "Optimization"
                },

                new QuestionTemplate
                {
                    Id = 309,
                    ProfessionId = professionId,
                    Question = "Bildiğiniz simülasyon araçlarını seçin.",
                    QuestionType = "MultiSelect",
                    IsRequired = false,
                    Category = "Simulation"
                },

                new QuestionTemplate
                {
                    Id = 310,
                    ProfessionId = professionId,
                    Question = "Daha önce geliştirdiğiniz bir proje var mı?",
                    QuestionType = "Select",
                    IsRequired = true,
                    Category = "Project"
                },

                new QuestionTemplate
                {
                    Id = 311,
                    ProfessionId = professionId,
                    Question = "Bildiğiniz yabancı dilleri seçin.",
                    QuestionType = "MultiSelect",
                    IsRequired = false,
                    Category = "Language"
                },

                new QuestionTemplate
                {
                    Id = 312,
                    ProfessionId = professionId,
                    Question = "Şirket veya kurum adı nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Experience"
                },

                new QuestionTemplate
                {
                    Id = 313,
                    ProfessionId = professionId,
                    Question = "Pozisyonunuz nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Experience"
                },

                new QuestionTemplate
                {
                    Id = 314,
                    ProfessionId = professionId,
                    Question = "Sorumluluklarınız nelerdi?",
                    QuestionType = "TextArea",
                    IsRequired = true,
                    Category = "Experience"
                },

                new QuestionTemplate
                {
                    Id = 315,
                    ProfessionId = professionId,
                    Question = "Proje adı nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Project"
                },

                new QuestionTemplate
                {
                    Id = 316,
                    ProfessionId = professionId,
                    Question = "Projeyi kısaca açıklayın.",
                    QuestionType = "TextArea",
                    IsRequired = true,
                    Category = "Project"
                },

                new QuestionTemplate
                {
                    Id = 317,
                    ProfessionId = professionId,
                    Question = "Projede kullandığınız araç ve teknolojileri yazın.",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Project"
                },

                new QuestionTemplate
                {
                    Id = 318,
                    ProfessionId = professionId,
                    Question = "Sahip olduğunuz sertifikalar var mı?",
                    QuestionType = "Select",
                    IsRequired = false,
                    Category = "Certificate"
                },

                new QuestionTemplate
                {
                    Id = 319,
                    ProfessionId = professionId,
                    Question = "Sertifika adı nedir?",
                    QuestionType = "Text",
                    IsRequired = false,
                    Category = "Certificate"
                }
            });

            AddOptions(options, 302,
                "Ön Lisans",
                "Lisans",
                "Yüksek Lisans",
                "Doktora");

            AddOptions(options, 305,
                "Evet",
                "Hayır");

            AddOptions(options, 306,
                "SAP",
                "Oracle ERP",
                "Microsoft Dynamics",
                "Logo ERP");

            AddOptions(options, 307,
                "Excel",
                "Power BI",
                "Python",
                "R",
                "SQL",
                "Tableau");

            AddOptions(options, 308,
                "Linear Programming",
                "Integer Programming",
                "Genetic Algorithm",
                "MATLAB",
                "Python OR-Tools");

            AddOptions(options, 309,
                "Arena",
                "AnyLogic",
                "FlexSim",
                "Simul8",
                "MATLAB Simulink");

            AddOptions(options, 310,
                "Evet",
                "Hayır");

            AddOptions(options, 311,
                "İngilizce",
                "Almanca",
                "Fransızca",
                "İspanyolca");

            AddOptions(options, 318,
                "Evet",
                "Hayır");
        }

        // =========================================================
        // ELECTRICAL AND ELECTRONICS ENGINEERING
        // =========================================================

        private static void AddElectricalQuestions(
            List<QuestionTemplate> questions,
            List<QuestionOption> options)
        {
            const int professionId = 4;

            questions.AddRange(new[]
            {
                new QuestionTemplate
                {
                    Id = 400,
                    ProfessionId = professionId,
                    Question = "CV başlığınız nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Profile"
                },

                new QuestionTemplate
                {
                    Id = 401,
                    ProfessionId = professionId,
                    Question = "Kendinizi kısaca tanıtın.",
                    QuestionType = "TextArea",
                    IsRequired = false,
                    Category = "Profile"
                },

                new QuestionTemplate
                {
                    Id = 402,
                    ProfessionId = professionId,
                    Question = "En yüksek eğitim seviyeniz nedir?",
                    QuestionType = "Select",
                    IsRequired = true,
                    Category = "Education"
                },

                new QuestionTemplate
                {
                    Id = 403,
                    ProfessionId = professionId,
                    Question = "Üniversitenizin adı nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Education"
                },

                new QuestionTemplate
                {
                    Id = 404,
                    ProfessionId = professionId,
                    Question = "Bölümünüz nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Education"
                },

                new QuestionTemplate
                {
                    Id = 405,
                    ProfessionId = professionId,
                    Question = "Daha önce profesyonel iş deneyiminiz oldu mu?",
                    QuestionType = "Select",
                    IsRequired = true,
                    Category = "Experience"
                },

                new QuestionTemplate
                {
                    Id = 406,
                    ProfessionId = professionId,
                    Question = "Bildiğiniz programlama dillerini seçin.",
                    QuestionType = "MultiSelect",
                    IsRequired = false,
                    Category = "Programming"
                },

                new QuestionTemplate
                {
                    Id = 407,
                    ProfessionId = professionId,
                    Question = "Bildiğiniz embedded sistem teknolojilerini seçin.",
                    QuestionType = "MultiSelect",
                    IsRequired = false,
                    Category = "Embedded"
                },

                new QuestionTemplate
                {
                    Id = 408,
                    ProfessionId = professionId,
                    Question = "Bildiğiniz elektronik tasarım araçlarını seçin.",
                    QuestionType = "MultiSelect",
                    IsRequired = false,
                    Category = "Electronics"
                },

                new QuestionTemplate
                {
                    Id = 409,
                    ProfessionId = professionId,
                    Question = "Bildiğiniz donanım ve mikrodenetleyicileri seçin.",
                    QuestionType = "MultiSelect",
                    IsRequired = false,
                    Category = "Hardware"
                },

                new QuestionTemplate
                {
                    Id = 410,
                    ProfessionId = professionId,
                    Question = "Bildiğiniz simülasyon araçlarını seçin.",
                    QuestionType = "MultiSelect",
                    IsRequired = false,
                    Category = "Simulation"
                },

                new QuestionTemplate
                {
                    Id = 411,
                    ProfessionId = professionId,
                    Question = "Daha önce geliştirdiğiniz bir proje var mı?",
                    QuestionType = "Select",
                    IsRequired = true,
                    Category = "Project"
                },

                new QuestionTemplate
                {
                    Id = 412,
                    ProfessionId = professionId,
                    Question = "Bildiğiniz yabancı dilleri seçin.",
                    QuestionType = "MultiSelect",
                    IsRequired = false,
                    Category = "Language"
                },

                new QuestionTemplate
                {
                    Id = 413,
                    ProfessionId = professionId,
                    Question = "Şirket veya kurum adı nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Experience"
                },

                new QuestionTemplate
                {
                    Id = 414,
                    ProfessionId = professionId,
                    Question = "Pozisyonunuz nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Experience"
                },

                new QuestionTemplate
                {
                    Id = 415,
                    ProfessionId = professionId,
                    Question = "Sorumluluklarınız nelerdi?",
                    QuestionType = "TextArea",
                    IsRequired = true,
                    Category = "Experience"
                },

                new QuestionTemplate
                {
                    Id = 416,
                    ProfessionId = professionId,
                    Question = "Proje adı nedir?",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Project"
                },

                new QuestionTemplate
                {
                    Id = 417,
                    ProfessionId = professionId,
                    Question = "Projeyi kısaca açıklayın.",
                    QuestionType = "TextArea",
                    IsRequired = true,
                    Category = "Project"
                },

                new QuestionTemplate
                {
                    Id = 418,
                    ProfessionId = professionId,
                    Question = "Projede kullandığınız teknolojileri yazın.",
                    QuestionType = "Text",
                    IsRequired = true,
                    Category = "Project"
                },

                new QuestionTemplate
                {
                    Id = 419,
                    ProfessionId = professionId,
                    Question = "Sahip olduğunuz sertifikalar var mı?",
                    QuestionType = "Select",
                    IsRequired = false,
                    Category = "Certificate"
                },

                new QuestionTemplate
                {
                    Id = 420,
                    ProfessionId = professionId,
                    Question = "Sertifika adı nedir?",
                    QuestionType = "Text",
                    IsRequired = false,
                    Category = "Certificate"
                }
            });

            AddOptions(options, 402,
                "Ön Lisans",
                "Lisans",
                "Yüksek Lisans",
                "Doktora");

            AddOptions(options, 405,
                "Evet",
                "Hayır");

            AddOptions(options, 406,
                "C",
                "C++",
                "Python",
                "C#",
                "MATLAB",
                "Verilog",
                "VHDL");

            AddOptions(options, 407,
                "Arduino",
                "STM32",
                "ESP32",
                "Raspberry Pi",
                "ARM",
                "PIC",
                "FreeRTOS");

            AddOptions(options, 408,
                "Altium Designer",
                "KiCad",
                "EAGLE",
                "Proteus",
                "OrCAD");

            AddOptions(options, 409,
                "STM32",
                "Arduino",
                "ESP32",
                "PIC",
                "ARM Cortex",
                "FPGA",
                "Raspberry Pi");

            AddOptions(options, 410,
                "MATLAB Simulink",
                "LTspice",
                "PSIM",
                "PSpice",
                "Multisim");

            AddOptions(options, 411,
                "Evet",
                "Hayır");

            AddOptions(options, 412,
                "İngilizce",
                "Almanca",
                "Fransızca",
                "İspanyolca");

            AddOptions(options, 419,
                "Evet",
                "Hayır");
        }

        // =========================================================
        // OPTION HELPER
        // =========================================================

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