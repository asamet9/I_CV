using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Infrastructure.Configuration
{
    /// <summary>
    /// Gemini AI sağlayıcısının yapılandırma ayarlarını temsil eder.
    /// </summary>
    public class GeminiOptions
    {
        /// <summary>
        /// Gemini API anahtarı.
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;

        /// <summary>
        /// Kullanılacak Gemini modelinin adı.
        /// </summary>
        public string Model { get; set; } = "gemini-2.5-flash";
    }
}