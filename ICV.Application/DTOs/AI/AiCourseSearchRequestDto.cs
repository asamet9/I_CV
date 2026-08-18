using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Application.DTOs.AI
{
    public class AiCourseSearchRequestDto
    {
        public string SkillName { get; set; } = string.Empty;

        public int CurrentLevel { get; set; }

        public int TargetLevel { get; set; }

        public int PreferredDuration { get; set; }

        public bool WantsPaidCourse { get; set; }

        public bool WantsCertificate { get; set; }

        public string? Purpose { get; set; }
    }
}