using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Application.DTOs.QuestionTemplate
{
    public class CreateQuestionOptionRequestDto
    {
        public int QuestionTemplateId { get; set; }

        public string OptionText { get; set; } = string.Empty;

        public string? OptionValue { get; set; }

        public int OrderIndex { get; set; }
    }
}