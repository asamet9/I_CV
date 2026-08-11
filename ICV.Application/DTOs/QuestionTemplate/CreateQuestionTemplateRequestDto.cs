using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Application.DTOs.QuestionTemplate
{
    public class CreateQuestionTemplateRequestDto
    {
        public int ProfessionId { get; set; }

        public string Question { get; set; } = string.Empty;

        public string QuestionType { get; set; } = string.Empty;

        public bool IsRequired { get; set; }
    }
}