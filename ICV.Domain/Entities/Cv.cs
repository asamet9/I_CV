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
        public int UserID { get; set; }

        public int ProfessionId { get; set; }

        public CvSource Source { get; set; }

        public User User { get; set; } = null!;

        public Profession Profession { get; set; } = null!;

        public ICollection<CvSection> Sections { get; set; } = new List<CvSection>();

        // Bu CV için oluşturulan tüm AI önerileri
        public ICollection<SkillSuggestion> SkillSuggestions { get; set; } = new List<SkillSuggestion>();

    }
}
