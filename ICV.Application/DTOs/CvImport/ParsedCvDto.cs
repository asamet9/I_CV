using ICV.Application.DTOs.CvImport;

public class ParsedCvDto
{
    public string? Summary { get; set; }

    public List<ParsedEducationDto> Education { get; set; } = new();
    public List<ParsedExperienceDto> Experience { get; set; } = new();
    public List<ParsedSkillDto> Skills { get; set; } = new();
    public List<ParsedLanguageDto> Languages { get; set; } = new();
    public List<ParsedCertificateDto> Certificates { get; set; } = new();
    public List<ParsedProjectDto> Projects { get; set; } = new();
}