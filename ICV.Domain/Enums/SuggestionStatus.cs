using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Domain.Enums
{

    /// <summary>
    /// Kullanıcının AI önerisine verdiği durumu belirtir.
    /// </summary>

    public enum SuggestionStatus
    {

        // Henüz karar verilmedi
        Pending = 1,

        // Kullanıcı öneriyi kabul etti
        Accepted = 2,

        // Kullanıcı öneriyi reddetti
        Dismissed = 3

    }
}
