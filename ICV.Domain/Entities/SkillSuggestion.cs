using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Domain.Common;
using ICV.Domain.Enums;

namespace ICV.Domain.Entities
{
    public class SkillSuggestion : BaseEntity
    {

        // Bu öneri hangi CV için üretildi?
        public int CvId { get; set; }

        // AI'nın önerdiği beceri
        public string SuggestedSkill { get; set; } = string.Empty;

        // AI neden bunu önerdi?
        public string Reason { get; set; } = string.Empty;

        // İleride UI'da renk veya kategori göstermek için kullanılabilir.
        // Örn: Technical, Soft Skill, Cloud...
        public string? Category { get; set; }

        // Kullanıcının öneriye verdiği durum.
        // Varsayılan olarak Pending başlasın.
        public SuggestionStatus Status { get; set; } = SuggestionStatus.Pending;

        // Navigation Property
        // Bu öneri hangi CV'ye ait?
        public Cv Cv { get; set; } = null!;

        // Bu yetenek önerisi için önerilen kurslar
        public ICollection<CourseRecommendation> CourseRecommendations { get; set; }
            = new List<CourseRecommendation>();

    }
}
