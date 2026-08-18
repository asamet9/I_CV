using ICV.Domain.Common;
using ICV.Domain.Enums;

namespace ICV.Domain.Entities
{
    /// <summary>
    /// Kullanıcının belirli bir yeteneği geliştirmek için
    /// oluşturduğu öğrenme hedefini temsil eder.
    /// </summary>
    public class SkillDevelopmentGoal : BaseEntity
    {
        // Bu hedefin hangi kullanıcıya ait olduğunu belirtir.
        public int UserId { get; set; }

        // Kullanıcının geliştirmek istediği yeteneğin adını tutar.
        // Örneğin: CSS, Docker, React, Azure...
        public string SkillName { get; set; } = string.Empty;

        // Bu hedef bir AI skill önerisinden oluşturulduysa
        // hangi öneriden geldiğini tutar.
        public int? SkillSuggestionId { get; set; }

        // Kullanıcının bu skill için mevcut seviyesini tutar.
        public SkillLevel CurrentLevel { get; set; }

        // AI tarafından kullanıcı için önerilen hedef seviyeyi tutar.
        // Örneğin AI, CSS için Intermediate seviyesini önerebilir.
        public SkillLevel RecommendedTargetLevel { get; set; }

        // Kullanıcının gerçekten seçtiği hedef seviyeyi tutar.
        // Kullanıcı AI'nın önerisini değiştirebilir.
        public SkillLevel TargetLevel { get; set; }

        // Kullanıcının seçtiği gelişim süresini tutar.
        public DevelopmentDuration PreferredDuration { get; set; }

        // Kullanıcının ücretli eğitimlere açık olup olmadığını belirtir.
        public bool WantsPaidCourse { get; set; }

        // Kullanıcının eğitim sonunda sertifika isteyip istemediğini belirtir.
        public bool WantsCertificate { get; set; }

        // Kullanıcının bu yeteneği neden geliştirmek istediğini tutar.
        // Örneğin: İş bulmak, kariyer geliştirmek, proje geliştirmek...
        public string? Purpose { get; set; }

        // Hedefin mevcut durumunu belirtir.
        // Örneğin: Active, Completed, Cancelled...
        public GoalStatus Status { get; set; } = GoalStatus.Active;

        // Navigation Properties

        // Hedefin sahibi olan kullanıcıyı temsil eder.
        public User User { get; set; } = null!;

        // Bu hedef bir skill önerisinden geldiyse
        // ilgili SkillSuggestion entity'sini temsil eder.
        public SkillSuggestion? SkillSuggestion { get; set; }

        public ICollection<CourseRecommendation> CourseRecommendations { get; set; }
        = new List<CourseRecommendation>();
    }
}