using ICV.Domain.Enums;

namespace ICV.Application.DTOs.CourseRecommendation
{
    public class CourseRecommendationResponseDto
    {
        public int Id { get; set; }

        public int SkillDevelopmentGoalId { get; set; }

        public int CourseId { get; set; }

        // Course bilgilerinden gelir.
        public string Title { get; set; } = string.Empty;

        public string Provider { get; set; } = string.Empty;

        public CoursePrice Price { get; set; }

        public CourseLevel Level { get; set; }

        public int DurationWeeks { get; set; }

        public string Url { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public string? Category { get; set; }
    }
}