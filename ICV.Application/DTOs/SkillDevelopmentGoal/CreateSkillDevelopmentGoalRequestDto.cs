using ICV.Domain.Enums;

namespace ICV.Application.DTOs.SkillDevelopmentGoal
{
    /// <summary>
    /// Kullanıcının yeni bir skill geliştirme hedefi
    /// oluştururken API'ye göndereceği verileri temsil eder.
    /// </summary>
    public class CreateSkillDevelopmentGoalRequestDto
    {
        // Eğer hedef bir AI skill önerisinden geldiyse
        // ilgili önerinin ID'sini tutar.
        public int? SkillSuggestionId { get; set; }

        // Geliştirilmek istenen skill'in adını tutar.
        // Örneğin: CSS, Docker, React...
        public string SkillName { get; set; } = string.Empty;

        // Kullanıcının mevcut seviyesini belirtir.
        public SkillLevel CurrentLevel { get; set; }

        // Kullanıcının ulaşmak istediği hedef seviyeyi belirtir.
        // AI'nın önerdiği seviyeden farklı olabilir.
        public SkillLevel TargetLevel { get; set; }

        // Kullanıcının ne kadar uzun bir gelişim süreci
        // istediğini belirtir.
        public DevelopmentDuration PreferredDuration { get; set; }

        // Kullanıcının ücretli eğitimlere açık olup olmadığını belirtir.
        public bool WantsPaidCourse { get; set; }

        // Kullanıcının gelişim sonunda sertifika isteyip istemediğini belirtir.
        public bool WantsCertificate { get; set; }

        // Kullanıcının bu skill'i neden geliştirmek istediğini belirtir.
        // Örneğin: İş bulmak, kariyer geliştirmek...
        public string? Purpose { get; set; }
    }
}