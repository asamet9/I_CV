using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Application.DTOs.Profession
{
    public class ProfessionResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}