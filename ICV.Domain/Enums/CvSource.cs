using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Domain.Entities;
using ICV.Domain.Common;

namespace ICV.Domain.Enums
{

    /// <summary>
    /// CV'nin sisteme nasıl eklendiğini belirtir.
    /// </summary>
    /// 
    public enum CvSource
    {

        // Kullanıcı PDF/DOCX yükledi
        Uploaded = 1,

        // Kullanıcı soruları cevaplayarak oluşturdu
        Created = 2

    }
}
