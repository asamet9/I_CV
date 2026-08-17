using ICV.Domain.Enums;

namespace ICV.Application.DTOs.AI
{
    /// <summary>
    /// AI tarafından CV analizi sonucunda
    /// kullanıcıya önerilen geliştirme yeteneğini temsil eder.
    /// </summary>
    public class AiSkillSuggestionDto
    {
        /// <summary>
        /// Önerilen yeteneğin adı.
        /// Örn: Docker
        /// </summary>
        public string Skill { get; set; } = string.Empty;

        /// <summary>
        /// Yeteneğin ait olduğu kategori.
        /// Örn: DevOps, Backend, Database
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// AI'ın bu yeteneği neden önerdiğini açıklayan gerekçe.
        /// </summary>
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// AI'ın kullanıcı için önerdiği hedef seviye.
        /// </summary>
        public SkillLevel RecommendedTargetLevel { get; set; }
    }
}