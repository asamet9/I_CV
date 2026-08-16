using ICV.Application.DTOs.AI; // AI test request DTO'suna erişmemizi sağlar.
using ICV.Application.Interfaces.AI; // IAiProvider interface'ini kullanmamızı sağlar.
using Microsoft.AspNetCore.Mvc; // ApiController ve HTTP action özelliklerini kullanmamızı sağlar.

namespace ICV.Api.Controllers
{
    /// <summary>
    /// Gemini AI entegrasyonunu test etmek için kullanılan geçici controller'dır.
    /// Gerçek sistem akışının bir parçası değildir.
    /// </summary>
    [ApiController] // Controller'ın Web API controller'ı olduğunu belirtir.
    [Route("api/[controller]")] // Endpoint adresini api/AiTest şeklinde oluşturur.
    public class AiTestController : ControllerBase
    {
        private readonly IAiProvider _aiProvider; // AI işlemlerini gerçekleştirecek provider'ı tutar.

        /// <summary>
        /// AiTestController constructor'ıdır.
        /// </summary>
        public AiTestController(IAiProvider aiProvider)
        {
            _aiProvider = aiProvider; // Dependency Injection tarafından verilen AI provider'ı saklar.
        }

        /// <summary>
        /// Gemini'nin CV üzerinden skill önerisi üretmesini test eder.
        /// </summary>
        [HttpPost("skill-suggestions")] // POST /api/AiTest/skill-suggestions endpoint'ini oluşturur.
        public async Task<IActionResult> GenerateSkillSuggestions(
            [FromBody] AiSkillSuggestionTestRequestDto request, // Swagger'dan gelen request body'sini DTO'ya bağlar.
            CancellationToken cancellationToken) // İstek iptal edilirse işlemi durdurmak için kullanılır.
        {
            var suggestions = await _aiProvider.GenerateSkillSuggestionsAsync(
                request.CvContent, // Test request'indeki CV metnini Gemini'ye gönderir.
                request.ProfessionName, // Test request'indeki mesleği Gemini'ye gönderir.
                cancellationToken); // CancellationToken'ı provider'a aktarır.

            return Ok(suggestions); // Gemini'den gelen skill önerilerini HTTP 200 ile döndürür.
        }
    }
}