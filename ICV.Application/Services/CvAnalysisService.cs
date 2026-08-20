using ICV.Application.DTOs.CvAnalysis;
using ICV.Application.Interfaces.AI;
using ICV.Application.Interfaces.Services;
using ICV.Application.Interfaces.UnitOfWork;
using ICV.Domain.Entities;
using System.Text.Json;

namespace ICV.Application.Services
{
    public class CvAnalysisService : ICvAnalysisService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAiProvider _aiProvider;
        private readonly ISkillSuggestionService _skillSuggestionService;

        public CvAnalysisService(
            IUnitOfWork unitOfWork,
            IAiProvider aiProvider,
            ISkillSuggestionService skillSuggestionService)
        {
            _unitOfWork = unitOfWork;
            _aiProvider = aiProvider;
            _skillSuggestionService = skillSuggestionService;
        }

        public async Task<CvAnalysisResponseDto> AnalyzeAsync(
            int cvId,
            AnalyzeCvRequestDto request,
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

            var profession = await _unitOfWork.Professions
                .FirstOrDefaultAsync(x =>
                    x.Id == request.ProfessionId);

            if (profession == null)
            {
                throw new KeyNotFoundException(
                    "Belirtilen meslek bulunamadı.");
            }

            var cvForAi = await BuildCvForAiAsync(
                cvId,
                userId);

            if (!cvForAi.Sections.Any(x => x.Items.Any()) &&
                string.IsNullOrWhiteSpace(cvForAi.Summary))
            {
                throw new InvalidOperationException(
                    "Bu CV içerisinde analiz edilecek veri bulunamadı.");
            }

            // ---------------------------------------------------------
            // 1. CV'NİN GENEL ANALİZİ
            // ---------------------------------------------------------

            var aiResult = await _aiProvider.GenerateCvAnalysisAsync(
                cvForAi,
                profession.Name);

            var matchedSkills = aiResult.Strengths
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            // ---------------------------------------------------------
            // 2. KULLANICIYA ÖZEL GELİŞİM ÖNERİLERİ
            // ---------------------------------------------------------

            var cvContent = JsonSerializer.Serialize(
                cvForAi,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            var skillSuggestions =
                await _skillSuggestionService.GenerateFromAiAsync(
                    cvId,
                    cvContent,
                    profession.Name,
                    userId);

            // AI'ın gerçekten kullanıcıya önerdiği skill'ler
            // MissingSkills olarak response'a aktarılır.
            var missingSkills = skillSuggestions
                .Where(x => !string.IsNullOrWhiteSpace(x.SuggestedSkill))
                .Select(x => new MissingSkillDto
                {
                    Skill = x.SuggestedSkill,
                    Category = x.Category
                })
                .ToList();

            // ---------------------------------------------------------
            // 3. CV ANALİZ KAYDI
            // ---------------------------------------------------------

            var analysis = new CvAnalysis
            {
                CvId = cvId,
                ProfessionId = request.ProfessionId,

                MatchedSkillCount = matchedSkills.Count,

                MissingSkillCount = missingSkills.Count,

                Score = aiResult.Score
            };

            await _unitOfWork.CvAnalyses.AddAsync(analysis);

            await _unitOfWork.SaveChangesAsync();

            // ---------------------------------------------------------
            // 4. RESPONSE
            // ---------------------------------------------------------

            return new CvAnalysisResponseDto
            {
                Id = analysis.Id,
                CvId = analysis.CvId,
                ProfessionId = analysis.ProfessionId,

                ProfessionName = profession.Name,

                MatchedSkillCount = analysis.MatchedSkillCount,
                MissingSkillCount = analysis.MissingSkillCount,

                Score = analysis.Score,

                CreatedAt = analysis.CreatedAt,

                MatchedSkills = matchedSkills,

                MissingSkills = missingSkills
            };
        }

        private async Task<CvForAiAnalysisDto> BuildCvForAiAsync(
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

            var sections = await _unitOfWork.CvSections
                .FindAsync(x => x.CvId == cvId);

            var sectionIds = sections
                .Select(x => x.Id)
                .ToList();

            var items = await _unitOfWork.CvSectionItems
                .FindAsync(x =>
                    sectionIds.Contains(x.CvSectionId));

            var result = new CvForAiAnalysisDto
            {
                CvId = cv.Id,
                Title = cv.Title,
                Summary = cv.Summary
            };

            foreach (var section in sections.OrderBy(x => x.OrderIndex))
            {
                var sectionDto = new CvAiSectionDto
                {
                    SectionType = section.Type.ToString()
                };

                var sectionItems = items
                    .Where(x => x.CvSectionId == section.Id)
                    .ToList();

                foreach (var item in sectionItems)
                {
                    sectionDto.Items.Add(new CvAiItemDto
                    {
                        Title = item.Title,
                        Description = item.Description,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate
                    });
                }

                result.Sections.Add(sectionDto);
            }

            return result;
        }
    }
}