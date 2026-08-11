using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Domain.Enums;

namespace ICV.Application.DTOs.UserSkillProgress
{
    public class UpdateUserSkillProgressRequestDto
    {
        public UserSkillProgressStatus Status { get; set; }

        public DateTime? LastCheckedAt { get; set; }

        public int CheckIntervalDays { get; set; }
    }
}

