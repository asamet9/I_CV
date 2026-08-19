using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ICV.Application.DTOs.QuestionOption
{
    public class QuestionOptionResponseDto
    {
        public int Id { get; set; }

        public int QuestionTemplateId { get; set; }

        public string OptionText { get; set; } = string.Empty;

        public string? OptionValue { get; set; }

        public int OrderIndex { get; set; }
    }
}