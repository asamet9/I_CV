using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Application.DTOs.AI
{
    /// <summary>
    /// Gemini AI skill suggestion testinde kullanılacak request modelidir.
    /// </summary>
    public class AiSkillSuggestionTestRequestDto
    {
        public string CvContent { get; set; } = string.Empty; // Gemini'ye gönderilecek CV metnini tutar.

        public string ProfessionName { get; set; } = string.Empty; // CV'nin ait olduğu mesleği tutar.
    }
}