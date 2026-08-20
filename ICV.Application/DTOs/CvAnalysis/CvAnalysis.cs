namespace ICV.Application.DTOs.CvAnalysis
{
    public class AiCvAnalysisResultDto
    {
        public decimal Score { get; set; }

        public string Summary { get; set; } = string.Empty;

        public List<string> Strengths { get; set; } = new();

        public List<string> Weaknesses { get; set; } = new();

        public List<AiMissingSkillDto> MissingSkills { get; set; } = new();

        public List<string> Recommendations { get; set; } = new();
    }

    public class AiMissingSkillDto
    {
        public string Skill { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Reason { get; set; } = string.Empty;
    }
}