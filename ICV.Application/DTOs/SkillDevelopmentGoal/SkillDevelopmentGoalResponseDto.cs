using ICV.Domain.Enums;

namespace ICV.Application.DTOs.SkillDevelopmentGoal
{
    /// <summary>
    /// Skill geliştirme hedefinin API üzerinden
    /// kullanıcıya döndürülen halidir.
    /// </summary>
    public class SkillDevelopmentGoalResponseDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int? SkillSuggestionId { get; set; }

        public string SkillName { get; set; } = string.Empty;

        public SkillLevel CurrentLevel { get; set; }

        public SkillLevel TargetLevel { get; set; }

        public DateTime? TargetDate { get; set; }

        public int WeeklyHours { get; set; }

        public string? Purpose { get; set; }

        public GoalStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}