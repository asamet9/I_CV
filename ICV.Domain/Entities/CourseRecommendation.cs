using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Domain.Common;
using ICV.Domain.Enums;

namespace ICV.Domain.Entities
{
    public class CourseRecommendation
    {

        // Bu kurs önerisi hangi yetenek önerisine ait?
        public int SkillSuggestionId { get; set; }

        // Kursun adı
        public string Title { get; set; } = string.Empty;

        // Kursu sağlayan platform (Udemy, Coursera, BTK vb.)
        public string Provider { get; set; } = string.Empty;

        // Kurs ücretsiz mi ücretli mi?
        public CoursePrice Price { get; set; }

        // Kurs seviyesi
        public CourseLevel Level { get; set; }

        // Ortalama kaç hafta sürüyor?
        public int DurationWeeks { get; set; }

        // Kurs bağlantısı
        public string Url { get; set; } = string.Empty;

        // Navigation Property
        // Bu kurs önerisinin bağlı olduğu SkillSuggestion nesnesi
        public SkillSuggestion SkillSuggestion { get; set; } = null!;

    }
}
