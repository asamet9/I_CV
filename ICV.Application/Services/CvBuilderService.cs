using ICV.Application.Interfaces.Services;
using ICV.Application.Interfaces.UnitOfWork;
using ICV.Domain.Entities;
using ICV.Domain.Enums;

namespace ICV.Application.Services
{
    public class CvBuilderService : ICvBuilderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CvBuilderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task BuildFromAnswersAsync(
            int cvId,
            int userId)
        {
            // =====================================================
            // 1. CV KONTROLÜ
            // =====================================================

            var cv = await _unitOfWork.Cvs
                .FirstOrDefaultAsync(x =>
                    x.Id == cvId &&
                    x.UserId == userId);

            if (cv == null)
            {
                throw new UnauthorizedAccessException(
                    "Bu CV'ye erişim yetkiniz yok.");
            }


            // =====================================================
            // 2. KULLANICININ CEVAPLARINI GETİR
            // =====================================================

            var answers = await _unitOfWork.UserCvAnswers
                .FindAsync(x => x.CvId == cvId);

            if (!answers.Any())
            {
                throw new InvalidOperationException(
                    "Bu CV için henüz cevaplanmış soru bulunmuyor.");
            }


            // =====================================================
            // 3. SORULARI GETİR
            // =====================================================

            var questionIds = answers
                .Select(x => x.QuestionTemplateId)
                .Distinct()
                .ToList();

            var questions = await _unitOfWork.QuestionTemplates
                .FindAsync(x => questionIds.Contains(x.Id));


            // =====================================================
            // 4. CEVAPLARI CV YAPISINA DÖNÜŞTÜR
            // =====================================================

            foreach (var answer in answers)
            {
                var question = questions
                    .FirstOrDefault(x =>
                        x.Id == answer.QuestionTemplateId);

                if (question == null)
                    continue;

                if (string.IsNullOrWhiteSpace(answer.Answer))
                    continue;


                // =================================================
                // 5. CATEGORY → SECTION TYPE
                // =================================================

                var sectionType = GetSectionType(
                    question.Category);

                if (sectionType == null)
                    continue;


                // =================================================
                // 6. SECTION'I BUL / OLUŞTUR
                // =================================================

                var section = await GetOrCreateSectionAsync(
                    cvId,
                    sectionType.Value);


                // =================================================
                // 7. SKILL / LANGUAGE CEVAPLARINI PARÇALA
                // =================================================

                if (ShouldSplitAnswer(question.Category))
                {
                    var values = SplitAnswer(answer.Answer);

                    foreach (var value in values)
                    {
                        await CreateSectionItemIfNotExistsAsync(
                            section,
                            value);
                    }

                    continue;
                }


                // =================================================
                // 8. NORMAL CEVAPLAR
                // =================================================

                var title = GetItemTitle(
                    question.Question,
                    question.Category);

                await CreateSectionItemIfNotExistsAsync(
                    section,
                    title,
                    answer.Answer);
            }


            // =====================================================
            // 9. TÜM DEĞİŞİKLİKLERİ KAYDET
            // =====================================================

            await _unitOfWork.SaveChangesAsync();
        }


        // =========================================================
        // CATEGORY → CV SECTION TYPE
        // =========================================================

        private static CvSectionType? GetSectionType(
            string? category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return null;

            return category switch
            {
                "Education" => CvSectionType.Education,

                "Experience" => CvSectionType.Experience,

                "Programming" => CvSectionType.Skill,
                "Database" => CvSectionType.Skill,
                "Tools" => CvSectionType.Skill,
                "DevOps" => CvSectionType.Skill,

                "Language" => CvSectionType.Language,

                "Certificate" => CvSectionType.Certificate,

                "Project" => CvSectionType.Project,

                "Profile" => null,

                _ => null
            };
        }


        // =========================================================
        // HANGİ CEVAPLAR PARÇALANACAK?
        // =========================================================

        private static bool ShouldSplitAnswer(
            string? category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return false;

            return category switch
            {
                "Programming" => true,
                "Database" => true,
                "Tools" => true,
                "DevOps" => true,
                "Language" => true,

                _ => false
            };
        }


        // =========================================================
        // SECTION BUL / OLUŞTUR
        // =========================================================

        private async Task<CvSection> GetOrCreateSectionAsync(
            int cvId,
            CvSectionType sectionType)
        {
            var section = await _unitOfWork.CvSections
                .FirstOrDefaultAsync(x =>
                    x.CvId == cvId &&
                    x.Type == sectionType);

            if (section != null)
                return section;


            var existingSections = await _unitOfWork.CvSections
                .FindAsync(x => x.CvId == cvId);

            var nextOrderIndex = existingSections.Any()
                ? existingSections.Max(x => x.OrderIndex) + 1
                : 1;


            section = new CvSection
            {
                CvId = cvId,
                Type = sectionType,
                OrderIndex = nextOrderIndex
            };


            await _unitOfWork.CvSections
                .AddAsync(section);

            await _unitOfWork.SaveChangesAsync();

            return section;
        }


        // =========================================================
        // SECTION ITEM OLUŞTUR
        // =========================================================

        private async Task CreateSectionItemIfNotExistsAsync(
            CvSection section,
            string title,
            string? description = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                return;


            var normalizedTitle = title.Trim();

            var existingItems = await _unitOfWork.CvSectionItems
                .FindAsync(x =>
                    x.CvSectionId == section.Id);


            var existingItem = existingItems
                .FirstOrDefault(x =>
                    string.Equals(
                        x.Title.Trim(),
                        normalizedTitle,
                        StringComparison.OrdinalIgnoreCase));


            if (existingItem != null)
                return;


            var item = new CvSectionItem
            {
                CvSectionId = section.Id,
                Title = normalizedTitle,
                Description = description?.Trim()
            };


            await _unitOfWork.CvSectionItems
                .AddAsync(item);
        }


        // =========================================================
        // CEVABI PARÇALA
        // =========================================================

        private static IEnumerable<string> SplitAnswer(
            string? answer)
        {
            if (string.IsNullOrWhiteSpace(answer))
                return Enumerable.Empty<string>();

            return answer
                .Split(
                    new[] { ',', ';', '|', '\n' },
                    StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase);
        }


        // =========================================================
        // NORMAL CEVAPLAR İÇİN ITEM BAŞLIĞI
        // =========================================================

        private static string GetItemTitle(
            string question,
            string? category)
        {
            if (question.Contains(
                "üniversitenizin adı",
                StringComparison.OrdinalIgnoreCase))
            {
                return "Üniversite";
            }


            if (question.Contains(
                "bölümünüz",
                StringComparison.OrdinalIgnoreCase))
            {
                return "Bölüm";
            }


            if (question.Contains(
                "şirket veya kurum adı",
                StringComparison.OrdinalIgnoreCase))
            {
                return "Şirket";
            }


            if (question.Contains(
                "pozisyonunuz",
                StringComparison.OrdinalIgnoreCase))
            {
                return "Pozisyon";
            }


            if (question.Contains(
                "proje adı",
                StringComparison.OrdinalIgnoreCase))
            {
                return "Proje";
            }


            if (question.Contains(
                "sertifika adı",
                StringComparison.OrdinalIgnoreCase))
            {
                return "Sertifika";
            }


            if (question.Contains(
                "sorumluluklarınız",
                StringComparison.OrdinalIgnoreCase))
            {
                return "Sorumluluklar";
            }


            if (question.Contains(
                "projeyi kısaca açıklayın",
                StringComparison.OrdinalIgnoreCase))
            {
                return "Açıklama";
            }


            if (question.Contains(
                "projede kullandığınız teknolojileri",
                StringComparison.OrdinalIgnoreCase))
            {
                return "Teknolojiler";
            }


            if (question.Contains(
                "sertifikayı veren kurum",
                StringComparison.OrdinalIgnoreCase))
            {
                return "Veren Kurum";
            }


            return category ?? "Bilgi";
        }
    }
}