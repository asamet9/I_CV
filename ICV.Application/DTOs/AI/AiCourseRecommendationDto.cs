namespace ICV.Application.DTOs.AI
{
    public class AiCourseRecommendationDto
    {
        public string Title { get; set; } = string.Empty;

        public string Provider { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public string Reason { get; set; } = string.Empty;

        public int Level { get; set; }

        public bool IsFree { get; set; }

        public string? Category { get; set; }

        public int DurationHours { get; set; }
    }
}