using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Domain.Enums
{

    /// <summary>
    /// Kullanıcının önerilen beceri üzerindeki ilerleme durumu.
    /// </summary>

    public enum UserSkillProgressStatus
    {

        // Henüz başlanmadı.
        NotStarted = 1,

        // Devam ediyor.
        InProgress = 2,

        // Tamamlandı.
        Completed = 3,

        // Yarım bırakıldı.
        Abandoned = 4

    }
}
