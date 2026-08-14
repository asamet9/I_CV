using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Domain.Common;
using ICV.Domain.Enums;

namespace ICV.Domain.Entities
{
    public class CourseRecommendation : BaseEntity
    {
        // Bu kurs önerisi hangi yetenek önerisine ait?
        public int SkillSuggestionId { get; set; }

        // Bu öneride hangi kurs kullanılıyor?
        public int CourseId { get; set; }

        // Kursun adı
        public string Title { get; set; } = string.Empty;

        // Kursu sağlayan platform
        // Örn: Udemy, Coursera, BTK vb.
        public string Provider { get; set; } = string.Empty;

        // Kurs ücretsiz mi ücretli mi?
        public CoursePrice Price { get; set; }

        // Kurs seviyesi
        public CourseLevel Level { get; set; }

        // Ortalama kaç hafta sürüyor?
        public int DurationWeeks { get; set; }

        // Kurs bağlantısı
        public string Url { get; set; } = string.Empty;


        // Navigation Properties

        // Bu kurs önerisinin bağlı olduğu SkillSuggestion
        public SkillSuggestion SkillSuggestion { get; set; } = null!;

        // Bu öneride kullanılan gerçek Course
        public Course Course { get; set; } = null!;
    }
}