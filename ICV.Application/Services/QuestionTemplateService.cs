using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Application.DTOs.QuestionTemplate;
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

        public async Task<QuestionTemplateResponseDto> CreateAsync(
            CreateQuestionTemplateRequestDto request)
        {
            // Önce Profession'ın gerçekten var olup olmadığını kontrol ediyoruz.
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
                Category = request.Category
            };

            await _unitOfWork.QuestionTemplates
                .AddAsync(questionTemplate);

            await _unitOfWork.SaveChangesAsync();

            return new QuestionTemplateResponseDto
            {
                Id = questionTemplate.Id,
                ProfessionId = questionTemplate.ProfessionId,
                Question = questionTemplate.Question,
                QuestionType = questionTemplate.QuestionType,
                IsRequired = questionTemplate.IsRequired,
                Category = questionTemplate.Category,
                CreatedAt = questionTemplate.CreatedAt
            };
        }

        public async Task<IEnumerable<QuestionTemplateResponseDto>> GetAllAsync(
            int professionId)
        {
            var templates = await _unitOfWork.QuestionTemplates
                .FindAsync(x => x.ProfessionId == professionId);

            return templates.Select(x => new QuestionTemplateResponseDto
            {
                Id = x.Id,
                ProfessionId = x.ProfessionId,
                Question = x.Question,
                QuestionType = x.QuestionType,
                IsRequired = x.IsRequired,
                Category = x.Category,
                CreatedAt = x.CreatedAt
            });
        }

        public async Task<QuestionTemplateResponseDto?> GetByIdAsync(
            int questionTemplateId)
        {
            var questionTemplate = await _unitOfWork.QuestionTemplates
                .GetByIdAsync(questionTemplateId);

            if (questionTemplate == null)
                return null;

            return new QuestionTemplateResponseDto
            {
                Id = questionTemplate.Id,
                ProfessionId = questionTemplate.ProfessionId,
                Question = questionTemplate.Question,
                QuestionType = questionTemplate.QuestionType,
                IsRequired = questionTemplate.IsRequired,
                Category = questionTemplate.Category,
                CreatedAt = questionTemplate.CreatedAt
            };
        }

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
            questionTemplate.Category = request.Category;

            _unitOfWork.QuestionTemplates.Update(questionTemplate);

            await _unitOfWork.SaveChangesAsync();

            return new QuestionTemplateResponseDto
            {
                Id = questionTemplate.Id,
                ProfessionId = questionTemplate.ProfessionId,
                Question = questionTemplate.Question,
                QuestionType = questionTemplate.QuestionType,
                IsRequired = questionTemplate.IsRequired,
                Category = questionTemplate.Category,
                CreatedAt = questionTemplate.CreatedAt
            };
        }

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
    }
}

