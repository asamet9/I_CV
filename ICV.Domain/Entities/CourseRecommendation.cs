using ICV.Domain.Common;

namespace ICV.Domain.Entities
{
    public class CourseRecommendation : BaseEntity
    {
        public int SkillDevelopmentGoalId { get; set; }

        public int CourseId { get; set; }

        public SkillDevelopmentGoal SkillDevelopmentGoal { get; set; } = null!;

        public Course Course { get; set; } = null!;
    }
}