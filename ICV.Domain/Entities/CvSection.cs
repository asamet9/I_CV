using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Domain.Common;
using ICV.Domain.Enums;



namespace ICV.Domain.Entities
{
    public class CvSection : BaseEntity 
    {

        public int CvId { get; set; }
        public CvSectionType Type { get; set; }     // Bölüm tipi (Education, Experience vb.)

        public int OrderIndex { get; set; }

        public Cv Cv { get; set; } = null!;

        public ICollection<CvSectionItem> Items { get; set; } = new List<CvSectionItem>();     // Bu bölümün içerisindeki tüm kayıtlar



    }
}
