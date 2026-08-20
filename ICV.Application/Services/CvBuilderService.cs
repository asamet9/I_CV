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
            var cv = await _unitOfWork.Cvs
                .FirstOrDefaultAsync(x =>
                    x.Id == cvId &&
                    x.UserId == userId);

            if (cv == null)
            {
                throw new UnauthorizedAccessException(
                    "Bu CV'ye erişim yetkiniz yok.");
            }

            var answers = await _unitOfWork.UserCvAnswers
                .FindAsync(x => x.CvId == cvId);

            if (!answers.Any())
            {
                throw new InvalidOperationException(
                    "Bu CV için henüz cevaplanmış soru bulunmuyor.");
            }

            var questionIds = answers
                .Select(x => x.QuestionTemplateId)
                .Distinct()
                .ToList();

            var questions = await _unitOfWork.QuestionTemplates
                .FindAsync(x => questionIds.Contains(x.Id));

            foreach (var answer in answers)
            {
                var question = questions
                    .FirstOrDefault(x =>
                        x.Id == answer.QuestionTemplateId);

                if (question == null)
                    continue;

                if (string.IsNullOrWhiteSpace(answer.Answer))
                    continue;

                var sectionType = GetSectionType(
                    question.Category);

                if (sectionType == null)
                    continue;

                var section = await GetOrCreateSectionAsync(
                    cvId,
                    sectionType.Value);

                if (question.QuestionType == "MultiSelect")
                {
                    var values = SplitAnswer(answer.Answer);

                    foreach (var value in values)
                    {
                        await CreateSectionItemIfNotExistsAsync(
                            section,
                            value);
                    }
                }
                else
                {
                    var title = GetItemTitle(
                        question.Question,
                        question.Category);

                    await CreateSectionItemIfNotExistsAsync(
                        section,
                        title,
                        answer.Answer);
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }

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
                    x.Title == title &&
                    x.Description == description);

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
