using ICV.Application.DTOs.QuestionTemplate;
using ICV.Application.DTOs.QuestionOption;
using ICV.Application.Interfaces.Services;
using ICV.Application.Interfaces.UnitOfWork;
using ICV.Domain.Entities;

namespace ICV.Application.Services
{
    public class QuestionTemplateService : IQuestionTemplateService
    {
        private readonly IUnitOfWork _unitOfWork;

        public QuestionTemplateService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Yeni soru oluşturur.
        public async Task<QuestionTemplateResponseDto> CreateAsync(
            CreateQuestionTemplateRequestDto request)
        {
            // Profession gerçekten var mı?
            var profession = await _unitOfWork.Professions
                .GetByIdAsync(request.ProfessionId);

            if (profession == null)
                throw new KeyNotFoundException(
                    "Belirtilen profession bulunamadı.");

            var questionTemplate = new QuestionTemplate
            {
                ProfessionId = request.ProfessionId,
                Question = request.Question,
                QuestionType = request.QuestionType,
                IsRequired = request.IsRequired,
                ExpectedValue = request.ExpectedValue,
                Category = request.Category
            };

            await _unitOfWork.QuestionTemplates
                .AddAsync(questionTemplate);

            await _unitOfWork.SaveChangesAsync();

            return await MapToDtoAsync(questionTemplate);
        }

        // Bir mesleğe ait bütün soruları getirir.
        public async Task<IEnumerable<QuestionTemplateResponseDto>> GetAllAsync(
            int professionId)
        {
            var templates = await _unitOfWork.QuestionTemplates
                .FindAsync(x => x.ProfessionId == professionId);

            var result = new List<QuestionTemplateResponseDto>();

            foreach (var template in templates)
            {
                result.Add(await MapToDtoAsync(template));
            }

            return result;
        }

        // Tek bir soruyu getirir.
        public async Task<QuestionTemplateResponseDto?> GetByIdAsync(
            int questionTemplateId)
        {
            var questionTemplate = await _unitOfWork.QuestionTemplates
                .GetByIdAsync(questionTemplateId);

            if (questionTemplate == null)
                return null;

            return await MapToDtoAsync(questionTemplate);
        }

        // Soruyu günceller.
        public async Task<QuestionTemplateResponseDto?> UpdateAsync(
            int questionTemplateId,
            UpdateQuestionTemplateRequestDto request)
        {
            var questionTemplate = await _unitOfWork.QuestionTemplates
                .GetByIdAsync(questionTemplateId);

            if (questionTemplate == null)
                return null;

            questionTemplate.Question = request.Question;
            questionTemplate.QuestionType = request.QuestionType;
            questionTemplate.IsRequired = request.IsRequired;
            questionTemplate.ExpectedValue = request.ExpectedValue;
            questionTemplate.Category = request.Category;

            _unitOfWork.QuestionTemplates.Update(questionTemplate);

            await _unitOfWork.SaveChangesAsync();

            return await MapToDtoAsync(questionTemplate);
        }

        // Soruyu siler.
        // Cascade sayesinde bu soruya ait QuestionOption kayıtları da silinir.
        public async Task<bool> DeleteAsync(
            int questionTemplateId)
        {
            var questionTemplate = await _unitOfWork.QuestionTemplates
                .GetByIdAsync(questionTemplateId);

            if (questionTemplate == null)
                return false;

            _unitOfWork.QuestionTemplates.Delete(questionTemplate);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        // Entity -> DTO dönüşümü
        // Ayrıca sorunun seçeneklerini de getirir.
        private async Task<QuestionTemplateResponseDto> MapToDtoAsync(
            QuestionTemplate questionTemplate)
        {
            var options = await _unitOfWork.QuestionOptions
                .FindAsync(x =>
                    x.QuestionTemplateId == questionTemplate.Id);

            return new QuestionTemplateResponseDto
            {
                Id = questionTemplate.Id,
                ProfessionId = questionTemplate.ProfessionId,
                Question = questionTemplate.Question,
                QuestionType = questionTemplate.QuestionType,
                IsRequired = questionTemplate.IsRequired,
                ExpectedValue = questionTemplate.ExpectedValue,
                Category = questionTemplate.Category,
                CreatedAt = questionTemplate.CreatedAt,

                Options = options
                    .OrderBy(x => x.OrderIndex)
                    .Select(x => new QuestionOptionResponseDto
                    {
                        Id = x.Id,
                        QuestionTemplateId = x.QuestionTemplateId,
                        OptionText = x.OptionText,
                        OptionValue = x.OptionValue,
                        OrderIndex = x.OrderIndex
                    })
                    .ToList()
            };
        }
    }
}
