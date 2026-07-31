using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Domain.Common;

namespace ICV.Domain.Entities
{
    public class Profession : BaseEntity
    {

        public string Name {  get; set; } = string.Empty;
        
        public ICollection<QuestionTemplate> QuestionTemplates { get; set; } = new List<QuestionTemplate>(); // Bu mesleğe ait tüm soru şablonları
                                                                                                             // Bir Profession'ın birden fazla QuestionTemplate'i olabilir.
        
        public ICollection<Cv> Cvs { get; set; } = new List<Cv>();

    }
}
