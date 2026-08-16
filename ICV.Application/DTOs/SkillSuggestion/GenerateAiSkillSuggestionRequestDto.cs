using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Application.DTOs.SkillSuggestion
{
    /// <summary>
    /// Gemini AI ile CV skill analizi başlatmak için
    /// API'den alınan bilgileri temsil eder.
    /// </summary>
    public class GenerateAiSkillSuggestionRequestDto
    {
        public int CvId { get; set; } // Analiz edilecek CV'nin ID'sidir.

        public string CvContent { get; set; } = string.Empty; // AI'a gönderilecek CV metnidir.

        public string ProfessionName { get; set; } = string.Empty; // CV'nin analiz edileceği meslektir.
    }
}