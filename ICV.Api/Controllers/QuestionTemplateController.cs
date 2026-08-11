using ICV.Application.DTOs.QuestionTemplate;
using ICV.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ICV.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuestionTemplateController : ControllerBase
    {
        private readonly IQuestionTemplateService _questionTemplateService;

        public QuestionTemplateController(
            IQuestionTemplateService questionTemplateService)
        {
            _questionTemplateService = questionTemplateService;
        }

        // Yeni soru şablonu oluşturur.
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateQuestionTemplateRequestDto request)
        {
            try
            {
                var result = await _questionTemplateService
                    .CreateAsync(request);

                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(
                    "Belirtilen profession bulunamadı.");
            }
        }

        // Belirli bir profession'a ait tüm soru şablonlarını getirir.
        [HttpGet("profession/{professionId:int}")]
        public async Task<IActionResult> GetAll(int professionId)
        {
            var result = await _questionTemplateService
                .GetAllAsync(professionId);

            return Ok(result);
        }

        // Tek bir soru şablonunu getirir.
        [HttpGet("{questionTemplateId:int}")]
        public async Task<IActionResult> GetById(
            int questionTemplateId)
        {
            var result = await _questionTemplateService
                .GetByIdAsync(questionTemplateId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // Soru şablonunu günceller.
        [HttpPut("{questionTemplateId:int}")]
        public async Task<IActionResult> Update(
            int questionTemplateId,
            UpdateQuestionTemplateRequestDto request)
        {
            var result = await _questionTemplateService
                .UpdateAsync(questionTemplateId, request);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // Soru şablonunu siler.
        [HttpDelete("{questionTemplateId:int}")]
        public async Task<IActionResult> Delete(
            int questionTemplateId)
        {
            var result = await _questionTemplateService
                .DeleteAsync(questionTemplateId);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}

