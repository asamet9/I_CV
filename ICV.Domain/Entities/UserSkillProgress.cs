using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Domain.Common;
using ICV.Domain.Enums;

namespace ICV.Domain.Entities
{
    public class UserSkillProgress
    {

        // Hangi kullanıcı?
        public int UserId { get; set; }

        // Hangi AI önerisi?
        public int SkillSuggestionId { get; set; }

        // Kullanıcının ilerleme durumu.
        public UserSkillProgressStatus Status { get; set; } =
            UserSkillProgressStatus.NotStarted;

        // Kullanıcının en son ne zaman kontrol ettiği.
        public DateTime? LastCheckedAt { get; set; }

        // Kaç günde bir hatırlatma yapılsın?
        public int CheckIntervalDays { get; set; }

        // Navigation Property
        public User User { get; set; } = null!;

        // Navigation Property
        public SkillSuggestion SkillSuggestion { get; set; } = null!;

    }
}
