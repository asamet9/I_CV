using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Application.DTOs.CvImport
{
    public class ImportedCvDto
    {
        public int CvId { get; set; }

        public string Title { get; set; } = string.Empty;

        public int ProfessionId { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}