using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Application.DTOs.CvSection
{
    public class CvSectionResponseDto
    {
        public int Id { get; set; }

        public int CvId { get; set; }

        public int Type { get; set; }

        public int OrderIndex { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}