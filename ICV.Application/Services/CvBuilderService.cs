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
            // 4. CEVAPLARI CV'YE AKTAR
            // =====================================================

            foreach (var answer in answers)
            {
                var question = questions
                    .FirstOrDefault(x =>
                        x.Id == answer.QuestionTemplateId);

                if (question == null)
                    continue;

                // Cevabın bağlı olduğu kategoriye göre
                // hangi CV section'ına gideceğini buluyoruz.
                var sectionType = GetSectionType(question.Category);

                // Profile gibi CV section'ına dönüştürülmeyecek
                // kategorileri şimdilik atlıyoruz.
                if (sectionType == null)
                    continue;

                // İlgili section varsa onu getiriyoruz,
                // yoksa oluşturuyoruz.
                var section = await GetOrCreateSectionAsync(
                    cvId,
                    sectionType.Value);

                // -------------------------------------------------
                // MULTISELECT
                // -------------------------------------------------

                if (question.QuestionType == "MultiSelect")
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

                // -------------------------------------------------
                // NORMAL CEVAPLAR
                // -------------------------------------------------

                if (string.IsNullOrWhiteSpace(answer.Answer))
                    continue;

                await CreateSectionItemIfNotExistsAsync(
                    section,
                    GetTitle(question),
                    answer.Answer);
            }

            // =====================================================
            // 5. TÜM DEĞİŞİKLİKLERİ KAYDET
            // =====================================================

            await _unitOfWork.SaveChangesAsync();
        }

        // =========================================================
        // CATEGORY → CV SECTION
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

            // Section'ın ID'sini alabilmek için
            // burada SaveChanges yapıyoruz.
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

            var existingItem = await _unitOfWork.CvSectionItems
                .FirstOrDefaultAsync(x =>
                    x.CvSectionId == section.Id &&
                    x.Title == title);

            if (existingItem != null)
                return;

            var item = new CvSectionItem
            {
                CvSectionId = section.Id,
                Title = title.Trim(),
                Description = description?.Trim()
            };

            await _unitOfWork.CvSectionItems
                .AddAsync(item);
        }

        // =========================================================
        // MULTISELECT CEVABI PARÇALA
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
        // SORUYA GÖRE ITEM BAŞLIĞI
        // =========================================================

        private static string GetTitle(
            dynamic question)
        {
            var questionText =
                (string)question.Question;

            if (questionText.Contains(
                "üniversitenizin adı",
                StringComparison.OrdinalIgnoreCase))
            {
                return "Üniversite";
            }

            if (questionText.Contains(
                "bölümünüz",
                StringComparison.OrdinalIgnoreCase))
            {
                return "Bölüm";
            }

            if (questionText.Contains(
                "şirket veya kurum adı",
                StringComparison.OrdinalIgnoreCase))
            {
                return "Şirket";
            }

            if (questionText.Contains(
                "pozisyonunuz",
                StringComparison.OrdinalIgnoreCase))
            {
                return "Pozisyon";
            }

            if (questionText.Contains(
                "proje adı",
                StringComparison.OrdinalIgnoreCase))
            {
                return "Proje";
            }

            if (questionText.Contains(
                "sertifika adı",
                StringComparison.OrdinalIgnoreCase))
            {
                return "Sertifika";
            }

            return questionText;
        }
    }
}