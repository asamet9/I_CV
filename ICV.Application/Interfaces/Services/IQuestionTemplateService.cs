using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICV.Application.DTOs.QuestionTemplate;

namespace ICV.Application.Interfaces.Services
{
    public interface IQuestionTemplateService
    {
        Task<QuestionTemplateResponseDto> CreateAsync(
            CreateQuestionTemplateRequestDto request);

        Task<IEnumerable<QuestionTemplateResponseDto>> GetAllAsync(
            int professionId);

        Task<QuestionTemplateResponseDto?> GetByIdAsync(
            int questionTemplateId);

        Task<QuestionTemplateResponseDto?> UpdateAsync(
            int questionTemplateId,
            UpdateQuestionTemplateRequestDto request);

        Task<bool> DeleteAsync(
            int questionTemplateId);
    }
}