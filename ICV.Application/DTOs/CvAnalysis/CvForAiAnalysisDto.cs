namespace ICV.Application.DTOs.CvAnalysis
{
    public class CvForAiAnalysisDto
    {
        public int CvId { get; set; }

        public string? Title { get; set; }

        public string? Summary { get; set; }

        public List<CvAiSectionDto> Sections { get; set; } = new();
    }

    public class CvAiSectionDto
    {
        public string SectionType { get; set; } = string.Empty;

        public List<CvAiItemDto> Items { get; set; } = new();
    }

    public class CvAiItemDto
    {
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}