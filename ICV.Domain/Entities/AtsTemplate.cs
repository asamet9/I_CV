using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Domain.Common;
using ICV.Domain.Enums;

namespace ICV.Domain.Entities
{

    public class AtsTemplate : BaseEntity
    {
        // Şablonun adı
        public string Name { get; set; } = string.Empty;

        // Şablon tipi
        public TemplateType TemplateType { get; set; }
    }
}