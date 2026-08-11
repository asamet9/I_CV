using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Domain.Enums;

namespace ICV.Application.DTOs.CourseRecommendation
{
    public class UpdateCourseRecommendationRequestDto
    {
        public string Title { get; set; } = string.Empty;

        public string Provider { get; set; } = string.Empty;

        public CoursePrice Price { get; set; }

        public CourseLevel Level { get; set; }

        public int DurationWeeks { get; set; }

        public string Url { get; set; } = string.Empty;
    }
}
