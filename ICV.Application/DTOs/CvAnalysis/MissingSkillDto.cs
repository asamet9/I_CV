using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Application.DTOs.CvAnalysis
{
    /// <summary>
    /// CV analizinde eksik bulunan bir yeteneği
    /// kategori bilgisiyle birlikte taşır.
    /// </summary>
    public class MissingSkillDto
    {
        public string Skill { get; set; } = string.Empty;

        public string? Category { get; set; }
    }
}