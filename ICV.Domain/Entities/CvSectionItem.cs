using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Domain.Common;

namespace ICV.Domain.Entities
{
    public class CvSectionItem : BaseEntity 
    {
        // Bu kayıt hangi bölüme ait?
        public int CvSectionId { get; set; }


        // Başlık
        // Örn: "Bilgisayar Mühendisliği"
        //      "Backend Developer"
        public string Title { get; set; } = string.Empty;


        // Açıklama
        // Örn: İş tanımı, okul açıklaması vb.
        public string? Description { get; set; }

        // Başlangıç tarihi
        public DateTime? StartDate { get; set; }

        // Bitiş tarihi
        // Devam ediyorsa null kalabilir.
        public DateTime? EndDate { get; set; }


        // Navigation Property
        // Bu kayıt hangi CvSection'a ait?
        public CvSection CvSection { get; set; } = null!;

    }
}
