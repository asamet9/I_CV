using ICV.Domain.Enums;

namespace ICV.Application.DTOs.SkillDevelopmentGoal
{
    /// <summary>
    /// Mevcut bir skill geliştirme hedefinin
    /// güncellenmesi sırasında kullanılan DTO'dur.
    /// </summary>
    public class UpdateSkillDevelopmentGoalRequestDto
    {
        public SkillLevel CurrentLevel { get; set; }

        public SkillLevel TargetLevel { get; set; }

        public DevelopmentDuration PreferredDuration { get; set; }

        public bool WantsPaidCourse { get; set; }

        public bool WantsCertificate { get; set; }

        public string? Purpose { get; set; }

        public GoalStatus Status { get; set; }
    }
}