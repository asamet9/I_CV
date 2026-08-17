using ICV.Domain.Enums;

namespace ICV.Application.DTOs.SkillDevelopmentGoal
{
    /// <summary>
    /// Skill geliştirme hedefinin API üzerinden
    /// kullanıcıya döndürülen halidir.
    /// </summary>
    public class SkillDevelopmentGoalResponseDto
    {
        // Hedefin benzersiz ID'sidir.
        public int Id { get; set; }

        // Hedefin sahibi olan kullanıcının ID'sidir.
        public int UserId { get; set; }

        // Hedef bir AI önerisinden oluşturulduysa
        // ilgili SkillSuggestion ID'sini tutar.
        public int? SkillSuggestionId { get; set; }

        // Geliştirilen skill'in adıdır.
        public string SkillName { get; set; } = string.Empty;

        // Kullanıcının başlangıç seviyesidir.
        public SkillLevel CurrentLevel { get; set; }

        // AI tarafından önerilen hedef seviyedir.
        public SkillLevel RecommendedTargetLevel { get; set; }

        // Kullanıcının seçtiği gerçek hedef seviyedir.
        public SkillLevel TargetLevel { get; set; }

        // Kullanıcının seçtiği gelişim süresidir.
        public DevelopmentDuration PreferredDuration { get; set; }

        // Kullanıcı ücretli eğitimlere açık mı?
        public bool WantsPaidCourse { get; set; }

        // Kullanıcı sertifika istiyor mu?
        public bool WantsCertificate { get; set; }

        // Kullanıcının bu skill'i geliştirme amacı.
        public string? Purpose { get; set; }

        // Hedefin mevcut durumudur.
        public GoalStatus Status { get; set; }

        // Hedefin oluşturulma tarihidir.
        public DateTime CreatedAt { get; set; }
    }
}