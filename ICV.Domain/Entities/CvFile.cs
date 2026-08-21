using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Domain.Common;

namespace ICV.Domain.Entities
{
    public class CvFile : BaseEntity
    {
        // Hangi CV'ye ait?
        public int CvId { get; set; }

        // Kullanıcının yüklediği gerçek dosyanın adı
        public string OriginalFileName { get; set; } = string.Empty;

        // Storage'da kullanılan benzersiz dosya adı
        public string StoredFileName { get; set; } = string.Empty;

        // Storage içerisindeki dosya yolu
        public string StoragePath { get; set; } = string.Empty;

        // Örn: application/pdf
        public string ContentType { get; set; } = string.Empty;

        // Byte cinsinden dosya boyutu
        public long FileSize { get; set; }

        // Navigation
        public Cv Cv { get; set; } = null!;
    }
}