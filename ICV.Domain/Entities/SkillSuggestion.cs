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

        // Skill kategorisi
        public string? Category { get; set; }

        // AI'nın kullanıcı için önerdiği hedef seviye.
        public SkillLevel RecommendedTargetLevel { get; set; }

        // Kullanıcının öneriye verdiği durum.
        public SuggestionStatus Status { get; set; } = SuggestionStatus.Pending;

        // Navigation
        public Cv Cv { get; set; } = null!;

        public ICollection<CourseRecommendation> CourseRecommendations { get; set; }
            = new List<CourseRecommendation>();

        public ICollection<UserSkillProgress> UserSkillProgresses { get; set; }
            = new List<UserSkillProgress>();

        public ICollection<SkillDevelopmentGoal> SkillDevelopmentGoals { get; set; }
            = new List<SkillDevelopmentGoal>();
    }
}
