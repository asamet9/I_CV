using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Domain.Common;
using ICV.Domain.Enums;

namespace ICV.Domain.Entities
{
    public class Cv : BaseEntity 
    {
        // Bu CV hangi kullanıcıya ait?
        public int UserId { get; set; }

        // Bu CV hangi meslek için hazırlandı?
        public int ProfessionId { get; set; }

        // CV'nin başlığı
        public string Title { get; set; } = string.Empty;

        // Kısa profil özeti
        public string? Summary { get; set; }

        // CV nasıl oluşturuldu?
        public CvSource Source { get; set; }

        // Navigation Property (Gezinme Özelliği)
        public User User { get; set; } = null!;

        // Navigation Property (Gezinme Özelliği)
        public Profession Profession { get; set; } = null!;

        // CV'nin tüm bölümleri
        public ICollection<CvSection> Sections { get; set; } = new List<CvSection>();

        // AI'nin oluşturduğu öneriler
        public ICollection<SkillSuggestion> SkillSuggestions { get; set; } = new List<SkillSuggestion>();
    }
}
