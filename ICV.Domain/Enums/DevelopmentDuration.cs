using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Domain.Enums
{
    /// <summary>
    /// Kullanıcının seçtiği gelişim süresini belirtir.
    /// </summary>
    public enum DevelopmentDuration
    {
        // Kısa süreli gelişim.
        Short = 1,

        // Orta süreli gelişim.
        Medium = 2,

        // Uzun süreli gelişim.
        Long = 3
    }
}