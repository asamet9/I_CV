using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ICV.Application.DTOs.CvImport
{
    public class ImportCvRequestDto
    {
        public Stream FileStream { get; set; } = Stream.Null;

        public string FileName { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public int ProfessionId { get; set; }

        public string Title { get; set; } = string.Empty;
    }
}