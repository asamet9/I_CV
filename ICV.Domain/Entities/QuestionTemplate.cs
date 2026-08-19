using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Domain.Common;

namespace ICV.Domain.Entities
{
    public class QuestionTemplate : BaseEntity
    {
        public int ProfessionId { get; set; }

        public string Question { get; set; } = string.Empty;

        public string QuestionType { get; set; } = string.Empty;

        public bool IsRequired { get; set; }

        public Profession Profession { get; set; } = null!;

        public string? ExpectedValue { get; set; }

        public string? Category { get; set; }

        // Bu soruya ait seçenekler
        public ICollection<QuestionOption> Options { get; set; }
            = new List<QuestionOption>();

    }
}