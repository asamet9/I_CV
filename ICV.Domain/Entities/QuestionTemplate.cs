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

        public bool IsRequired { get; set; } //cevaplaması zorunlu mu? 

        public Profession Profession { get; set; } = null!;    // Navigation Property
                                                               // EF Core sayesinde QuestionTemplate -> Profession ilişkisini kurar.
                                                               // Veritabanında kolon oluşturmaz, kod tarafındaki ilişkiyi temsil eder.

    }
}
