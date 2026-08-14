using ICV.Domain.Common;
using ICV.Domain.Enums;

namespace ICV.Domain.Entities
{
    /// <summary>
    /// Kullanıcının belirli bir yeteneği geliştirmek için
    /// oluşturduğu öğrenme hedefini temsil eder.
    /// </summary>
    public class SkillDevelopmentGoal : BaseEntity
    {
        // Bu hedef hangi kullanıcıya ait?
        public int UserId { get; set; }

        // Kullanıcının geliştirmek istediği yetenek.
        // Örneğin: Docker, React, Azure...
        public string SkillName { get; set; } = string.Empty;

        // Bu hedef bir SkillSuggestion üzerinden oluşturulduysa
        // hangi öneriden geldiğini tutar.
        public int? SkillSuggestionId { get; set; }

        // Kullanıcının mevcut seviyesi.
        public SkillLevel CurrentLevel { get; set; }

        // Kullanıcının ulaşmak istediği seviye.
        public SkillLevel TargetLevel { get; set; }

        // Kullanıcının hedeflediği bitiş tarihi.
        public DateTime? TargetDate { get; set; }

        // Kullanıcının haftada bu skill için ayırabileceği süre.
        public int WeeklyHours { get; set; }

        // Kullanıcının bu yeteneği neden öğrenmek istediği.
        // Örneğin: İş bulmak, kariyer geliştirmek, proje geliştirmek...
        public string? Purpose { get; set; }

        // Hedefin mevcut durumu.
        public GoalStatus Status { get; set; } = GoalStatus.Active;

        // Navigation Properties

        // Hedefin sahibi olan kullanıcı.
        public User User { get; set; } = null!;

        // Bu hedef bir öneriden geldiyse ilgili öneri.
        public SkillSuggestion? SkillSuggestion { get; set; }
    }
}