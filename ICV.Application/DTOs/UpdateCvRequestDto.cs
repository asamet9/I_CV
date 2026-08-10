using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Application.DTOs.Cv
{
    public class UpdateCvRequestDto
    {
        public int ProfessionId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Summary { get; set; }
    }
}