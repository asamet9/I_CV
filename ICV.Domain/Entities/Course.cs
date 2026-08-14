using ICV.Domain.Common;
using ICV.Domain.Enums;

namespace ICV.Domain.Entities
{
    /// <summary>
    /// Sistemde önerilebilecek bir öğrenme kursunu temsil eder.
    /// </summary>
    public class Course : BaseEntity
    {
        // Kursun adı.
        public string Title { get; set; } = string.Empty;

        // Kursun açıklaması.
        public string? Description { get; set; }

        // Kursu sağlayan platform.
        // Örn: Udemy, Coursera, YouTube...
        public string Provider { get; set; } = string.Empty;

        // Kursun bulunduğu adres.
        public string Url { get; set; } = string.Empty;

        // Kursun seviyesi.
        // SkillDevelopmentGoal ile aynı SkillLevel enum'ını kullanıyoruz.
        public SkillLevel Level { get; set; }

        // Kurs kategorisi.
        // Örn: Backend, DevOps, Cloud...
        public string? Category { get; set; }

        // Tahmini toplam kurs süresi.
        public int? DurationHours { get; set; }

        // Kurs ücretsiz mi?
        public bool IsFree { get; set; }

        // Kurs hâlâ önerilebilir durumda mı?
        public bool IsActive { get; set; } = true;

        // Bu kurs hangi öneriler içerisinde kullanılmış?
        public ICollection<CourseRecommendation> CourseRecommendations { get; set; }
            = new List<CourseRecommendation>();
    }
}