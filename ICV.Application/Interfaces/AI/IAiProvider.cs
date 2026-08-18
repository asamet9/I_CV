using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICV.Application.DTOs.AI;

namespace ICV.Application.Interfaces.AI
{
    /// <summary>
    /// AI sağlayıcıları için ortak abstraction.
    ///
    /// Application katmanı Gemini, OpenAI veya başka bir
    /// AI sağlayıcısının nasıl çalıştığını bilmez.
    /// Sadece bu interface üzerinden AI'dan sonuç ister.
    /// </summary>
    public interface IAiProvider
    {
        /// <summary>
        /// Verilen CV içeriğini ve hedef mesleği analiz ederek
        /// geliştirilmesi önerilen yetenekleri üretir.
        /// </summary>
        /// <param name="cvContent">
        /// Analiz edilecek CV'nin metinsel içeriği.
        /// </param>
        /// <param name="professionName">
        /// CV'nin ait olduğu meslek.
        /// </param>
        /// <param name="cancellationToken">
        /// Asenkron işlemin iptal edilmesini sağlar.
        /// </param>
        /// <returns>
        /// AI tarafından oluşturulan skill önerileri.
        /// </returns>
        Task<IEnumerable<AiSkillSuggestionDto>> GenerateSkillSuggestionsAsync(
            string cvContent,
            string professionName,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<AiCourseRecommendationDto>> GenerateCourseRecommendationsAsync(
    AiCourseSearchRequestDto request,
    CancellationToken cancellationToken = default);
    }
}