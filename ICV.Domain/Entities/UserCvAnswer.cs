using ICV.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Domain.Entities
{
    public class UserCvAnswer : BaseEntity
    {
        public int CvId { get; set; }

        public int QuestionTemplateId { get; set; }

        public string Answer { get; set; } = string.Empty;

        public Cv Cv { get; set; } = null!;

        public QuestionTemplate QuestionTemplate { get; set; } = null!;
    }
}
