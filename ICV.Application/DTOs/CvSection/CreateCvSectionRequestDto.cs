using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Application.DTOs.CvSection
{
    public class CreateCvSectionRequestDto
    {
        public int Type { get; set; }

        public int OrderIndex { get; set; }
    }
}