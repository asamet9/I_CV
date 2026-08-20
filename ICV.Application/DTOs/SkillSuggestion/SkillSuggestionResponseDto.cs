using ICV.Domain.Enums;

namespace ICV.Application.DTOs.SkillSuggestion
{
    public class SkillSuggestionResponseDto
    {
        public int Id { get; set; }

        public int CvId { get; set; }

        public string SuggestedSkill { get; set; } = string.Empty;

        public string Reason { get; set; } = string.Empty;

        public string? Category { get; set; }

        public SkillLevel RecommendedTargetLevel { get; set; }

        public int Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}