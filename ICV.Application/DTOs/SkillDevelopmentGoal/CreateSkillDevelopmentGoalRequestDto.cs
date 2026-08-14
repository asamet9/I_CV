using ICV.Domain.Enums;

namespace ICV.Application.DTOs.SkillDevelopmentGoal
{
    /// <summary>
    /// Kullanıcının yeni bir skill geliştirme hedefi
    /// oluştururken API'ye göndereceği verileri temsil eder.
    /// </summary>
    public class CreateSkillDevelopmentGoalRequestDto
    {
        // Eğer hedef bir AI önerisinden geldiyse
        // ilgili önerinin ID'si gönderilir.
        //
        // Kullanıcı kendi istediği bir skill'i de ekleyebileceği
        // için bu alan zorunlu değildir.
        public int? SkillSuggestionId { get; set; }

        // Geliştirilmek istenen skill.
        // Örn: Docker
        public string SkillName { get; set; } = string.Empty;

        // Kullanıcının mevcut seviyesi.
        public SkillLevel CurrentLevel { get; set; }

        // Kullanıcının ulaşmak istediği seviye.
        public SkillLevel TargetLevel { get; set; }

        // Hedeflenen bitiş tarihi.
        public DateTime? TargetDate { get; set; }

        // Haftada kaç saat çalışabileceği.
        public int WeeklyHours { get; set; }

        // Kullanıcının bu skill'i neden öğrenmek istediği.
        public string? Purpose { get; set; }
    }
}