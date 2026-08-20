using ICV.Application.DTOs.CvAnalysis;

public class CvAnalysisResponseDto
{
    public int Id { get; set; }

    public int CvId { get; set; }

    public int ProfessionId { get; set; }

    public string ProfessionName { get; set; } = string.Empty;

    public int MatchedSkillCount { get; set; }

    public int MissingSkillCount { get; set; }

    public decimal Score { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<string> MatchedSkills { get; set; } = new();

    public List<MissingSkillDto> MissingSkills { get; set; } = new();
}