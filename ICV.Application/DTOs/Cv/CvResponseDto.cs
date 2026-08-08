using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Application.DTOs.Cv
{
    public class CvResponseDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProfessionId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Summary { get; set; }

        public int Source { get; set; }

        public DateTime CreatedAt { get; set; }


    }
}
